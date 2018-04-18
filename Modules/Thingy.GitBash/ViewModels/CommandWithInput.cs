using System;
using System.Windows.Input;
using Thingy.API;
using Thingy.GitBash.Dialogs;

namespace Thingy.GitBash.ViewModels
{
    public enum InputCommands
    {
        Clone,
        AddAndCommit,
        PushPull,
        Tree,
        Bigtree,
        ChangeBranch,
        MergeBranch,
        CreateBranch,
        DeleteBranch,
        CreateTag
    }

    public class ComplexCommand : ICommand
    {
        private IApplication _app;
        private IGitBashView _view;


        public ComplexCommand(IApplication app, IGitBashView view)
        {
            _app = app;
            _view = view;
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            var dialog = new StringInputDialog();

            var command = (InputCommands)parameter;
            switch (command)
            {
                case InputCommands.Clone:
                    {
                        dialog.Description = "Enter repository url that you wish to clone";
                        var result = await _app.ShowDialog("Clone Repository", dialog, DialogButtons.OkCancel, false);
                        if (result)
                        {
                            _view.SendText($"git clone {dialog.Input}");
                        }
                    }
                    break;
                case InputCommands.AddAndCommit:
                    {
                        dialog.Description = "Enter Commit message";
                        var result = await _app.ShowDialog("Add files & commit", dialog, DialogButtons.OkCancel, false);
                        if (result)
                        {
                            _view.SendText($"git add .");
                            _view.SendText($"git commit -m \"{dialog.Input}\"");
                        }
                    }
                    break;
                case InputCommands.PushPull:
                    _view.SendText($"git pull");
                    _view.SendText($"git push");
                    break;
                case InputCommands.Tree:
                    _view.SendText("git log --pretty=format:'%Cred%h%Creset %C(bold blue)<%an>%Creset%C(yellow)%d%Creset %Cgreen(%cr)%Creset%n%w(80,8,8)%s' --graph");
                    break;
                case InputCommands.Bigtree:
                    _view.SendText("git log --pretty=format:'%Cred%h%Creset %C(bold blue)<%an>%Creset%C(yellow)%d%Creset %Cgreen(%cr)%Creset%n%w(80,8,8)%s%n' --graph --name-status");
                    break;
                case InputCommands.ChangeBranch:
                    {
                        dialog.Description = "Enter branch name to be checked out";
                        var result = await _app.ShowDialog("Change branch", dialog, DialogButtons.OkCancel, false);
                        if (result)
                        {
                            _view.SendText($"git checkout {dialog.Input}");
                        }
                    }
                    break;
                case InputCommands.MergeBranch:
                    {
                        dialog.Description = "Enter source branch to be merged to current branch";
                        var result = await _app.ShowDialog("Merge branch", dialog, DialogButtons.OkCancel, false);
                        if (result)
                        {
                            _view.SendText($"git merge {dialog.Input}");
                        }
                    }
                    break;
                case InputCommands.CreateBranch:
                    {
                        dialog.Description = "Enter new branch name";
                        var result = await _app.ShowDialog("Create branch", dialog, DialogButtons.OkCancel, false);
                        if (result)
                        {
                            _view.SendText($"git branch --set-upstream {dialog.Input}");
                            _view.SendText($"git checkout {dialog.Input}");
                        }
                    }
                    break;
                case InputCommands.DeleteBranch:
                    {
                        dialog.Description = "Enter branch name, that you wish to delete.\nWARNING: This can't be undone";
                        var result = await _app.ShowDialog("Delete branch", dialog, DialogButtons.OkCancel, false);
                        if (result)
                        {
                            _view.SendText($"git branch -d {dialog.Input}");
                        }
                    }
                    break;
                case InputCommands.CreateTag:
                    {
                        dialog.Description = "Enter new tag";
                        var result = await _app.ShowDialog("Create tag", dialog, DialogButtons.OkCancel, false);
                        if (result)
                        {
                            _view.SendText($"git tag {dialog.Input}");
                        }
                    }
                    break;

            }
        }
    }
}
