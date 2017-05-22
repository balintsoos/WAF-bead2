using Microsoft.Win32;
using System;
using System.Windows;
using Desktop.Model;
using Desktop.ViewModel;
using Desktop.View;

namespace Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IDesktopModel _model;
        private LoginViewModel _loginViewModel;
        private LoginWindow _loginView;
        private MainViewModel _mainViewModel;
        private MainWindow _mainView;
        private ArticleEditorWindow _editorView;

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
            Exit += new ExitEventHandler(App_Exit);
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _model = new DesktopModel(new NewsService("http://localhost:53634")); // megadjuk a szolgáltatás címét

            _loginViewModel = new LoginViewModel(_model);
            _loginViewModel.ExitApplication += new EventHandler(ViewModel_ExitApplication);
            _loginViewModel.LoginSuccess += new EventHandler(ViewModel_LoginSuccess);
            _loginViewModel.LoginFailed += new EventHandler(ViewModel_LoginFailed);

            _loginView = new LoginWindow();
            _loginView.DataContext = _loginViewModel;
            _loginView.Show();
        }

        private async void App_Exit(object sender, ExitEventArgs e)
        {
            if (_model.IsUserLoggedIn) // amennyiben be vagyunk jelentkezve, kijelentkezünk
            {
                await _model.LogoutAsync();
            }
        }

        private void ViewModel_LoginSuccess(object sender, EventArgs e)
        {
            _mainViewModel = new MainViewModel(_model);
            _mainViewModel.MessageApplication += new EventHandler<MessageEventArgs>(ViewModel_MessageApplication);
            _mainViewModel.ArticleEditingStarted += new EventHandler(MainViewModel_ArticleEditingStarted);
            _mainViewModel.ArticleEditingFinished += new EventHandler(MainViewModel_ArticleEditingFinished);
            _mainViewModel.ImageEditingStarted += new EventHandler<ArticleEventArgs>(MainViewModel_ImageEditingStarted);
            _mainViewModel.ExitApplication += new EventHandler(ViewModel_ExitApplication);

            _mainView = new MainWindow();
            _mainView.DataContext = _mainViewModel;
            _mainView.Show();

            _loginView.Close();
        }

        private void ViewModel_LoginFailed(object sender, EventArgs e)
        {
            MessageBox.Show("A bejelentkezés sikertelen!", "Random News", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void ViewModel_MessageApplication(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "Random News", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void MainViewModel_ArticleEditingStarted(object sender, EventArgs e)
        {
            _editorView = new ArticleEditorWindow(); // külön szerkesztő dialógus az épületekre
            _editorView.DataContext = _mainViewModel;
            _editorView.Show();
        }

        private void MainViewModel_ArticleEditingFinished(object sender, EventArgs e)
        {
            _editorView.Close();
        }

        private void MainViewModel_ImageEditingStarted(object sender, ArticleEventArgs e)
        {
            try
            {
                // egy dialógusablakban bekérjük a fájlnevet
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.CheckFileExists = true;
                dialog.Filter = "Képfájlok|*.jpg;*.jpeg;*.bmp;*.tif;*.gif;*.png;";
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                Boolean? result = dialog.ShowDialog();

                if (result == true)
                {
                    // kép létrehozása (a megfelelő méretekkel)
                    _model.CreateImage(e.ArticleId,
                                       ImageHandler.OpenAndResize(dialog.FileName, 100),
                                       ImageHandler.OpenAndResize(dialog.FileName, 600));
                }
            }
            catch { }
        }

        private void ViewModel_ExitApplication(object sender, System.EventArgs e)
        {
            Shutdown();
        }
    }
}
