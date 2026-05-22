using System;
using System.Threading.Tasks;
using Common.Utils;
using PcgTools.MasterFiles;
using PcgTools.Model.Common.File;
using PcgTools.Model.Common.Synth.MemoryAndFactory;
using PcgTools.Model.Common.Synth.SongsRelated;
using PcgTools.PcgToolsResources;
using PcgTools.Properties;
using WPF.MDI;

namespace PcgTools.ViewModels.Commands
{
    /// <summary>
    /// Utility class.
    /// </summary>
    public static class PcgFileCommands
    {
        public static async Task LoadFileAndMasterFileAsync(IMainViewModel mainViewModel, string fileName, bool checkAutoLoadMasterFileSetting)
        {
            // Parse the file on a background thread; all model objects created here are plain CLR
            // and have no WPF thread affinity until bound to the UI after this await.
            var korgFileReader = new KorgFileReader();
            var memory = await Task.Run(() => korgFileReader.Read(fileName));

            if (memory == null)
            {
                mainViewModel.ShowMessageBox(
                    string.Format(Strings.FileTypeNotSupportedForThisWorkstation,
                        Memory.FileTypeAsString(korgFileReader.FileType),
                        Model.Common.Synth.MemoryAndFactory.Model.ModelTypeAsString(korgFileReader.ModelType)),
                    Strings.PcgTools, WindowUtils.EMessageBoxButton.Ok, WindowUtils.EMessageBoxImage.Error,
                    WindowUtils.EMessageBoxResult.Ok);
                return;
            }

            mainViewModel.SelectedMemory = memory;

            // Pass memory.Model directly to avoid reading SelectedMemory from a concurrent load.
            await LoadMasterFileIfRequestedAsync(mainViewModel, checkAutoLoadMasterFileSetting, fileName, memory.Model);

            // Create child window (UI thread, safe after await).
            MdiChild mdiChild;
            if (memory is IPcgMemory)
            {
                var width = Settings.Default.UI_PcgWindowWidth == 0 ? 700 : Settings.Default.UI_PcgWindowWidth;
                var height = Settings.Default.UI_PcgWindowHeight == 0 ? 500 : Settings.Default.UI_PcgWindowHeight;
                mdiChild = mainViewModel.CreateMdiChildWindow(fileName, MainViewModel.ChildWindowType.Pcg, memory, width, height);
                ((PcgWindow)(mdiChild.Content)).ViewModel.SelectedMemory = memory;
                mainViewModel.CurrentChildViewModel = ((PcgWindow)(mdiChild.Content)).ViewModel;
                ((IPcgMemory)memory).SelectFirstBanks();
            }
            else if (memory is ISongMemory)
            {
                var width = Settings.Default.UI_SongWindowWidth == 0 ? 700 : Settings.Default.UI_SongWindowWidth;
                var height = Settings.Default.UI_SongWindowHeight == 0 ? 500 : Settings.Default.UI_SongWindowHeight;
                mdiChild = mainViewModel.CreateMdiChildWindow(fileName, MainViewModel.ChildWindowType.Song, memory, width, height);
                mainViewModel.CurrentChildViewModel = ((SongWindow)(mdiChild.Content)).ViewModel;
                ((SongWindow)(mdiChild.Content)).ViewModel.SelectedMemory = memory;
            }
            else
            {
                throw new ApplicationException("Unknown memory type");
            }
        }


        static async Task LoadMasterFileIfRequestedAsync(IMainViewModel mainViewModel, bool checkAutoLoadMasterFileSetting,
            string loadedPcgFileName, IModel model)
        {
            if (!checkAutoLoadMasterFileSetting)
            {
                return;
            }

            var masterFile = MasterFiles.MasterFiles.Instances.FindMasterFile(model);
            if ((masterFile == null) || (masterFile.FileState != MasterFile.EFileState.Unloaded))
            {
                return;
            }

            switch ((MasterFiles.MasterFiles.AutoLoadMasterFiles)(Settings.Default.MasterFiles_AutoLoad))
            {
                case MasterFiles.MasterFiles.AutoLoadMasterFiles.Always:
                    if (masterFile.FileName != loadedPcgFileName)
                    {
                        await LoadFileAndMasterFileAsync(mainViewModel, masterFile.FileName, false);
                    }
                    break;

                case MasterFiles.MasterFiles.AutoLoadMasterFiles.Ask:
                    if (masterFile.FileName != loadedPcgFileName)
                    {
                        var result = mainViewModel.ShowMessageBox(
                            string.Format(Strings.AskForMasterFile, masterFile.FileName),
                            Strings.PcgTools, WindowUtils.EMessageBoxButton.YesNo,
                            WindowUtils.EMessageBoxImage.Information,
                            WindowUtils.EMessageBoxResult.Yes);

                        if (result == WindowUtils.EMessageBoxResult.Yes)
                        {
                            await LoadFileAndMasterFileAsync(mainViewModel, masterFile.FileName, false);
                        }
                    }
                    break;

                case MasterFiles.MasterFiles.AutoLoadMasterFiles.Never:
                    break;

                default:
                    throw new ApplicationException("Illegal case");
            }
        }
    }
}
