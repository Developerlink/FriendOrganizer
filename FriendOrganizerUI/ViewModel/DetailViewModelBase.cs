using FriendOrganizerUI.Event;
using FriendOrganizerUI.View.Services;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizerUI.ViewModel
{
    public abstract class DetailViewModelBase : BaseViewModel, IDetailViewModel
    {
        private bool _hasChanges;
        protected readonly IEventAggregator EventAggregator;
        protected readonly IMessageDialogService MessageDialogService;
        public DetailViewModelBase(IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            EventAggregator = eventAggregator;
            MessageDialogService = messageDialogService;
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
            CloseDetailViewCommand = new DelegateCommand(OnCloseDetailViewExecute);
        }

        public abstract Task LoadAsync(int id);
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand DeleteCommand { get; }
        public DelegateCommand CloseDetailViewCommand { get; }

        private int _id;

        public int Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            protected set
            {
                _title = value;
                OnPropertyChanged();
            }
        }


        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        protected abstract void OnDeleteExecute();

        protected abstract bool OnSaveCanExecute();

        protected abstract void OnSaveExecute();

        protected virtual void RaiseDetailDeletedEvent(int modelId)
        {
            EventAggregator.GetEvent<AfterDetailDeletedEvent>().Publish(new
                AfterDetailDeletedEventArgs
            {
                Id = modelId,
                ViewModelName = this.GetType().Name
            });
        }

        protected virtual void RaiseDetailSavedEvent(int modelId, string displayMemeber)
        {
            EventAggregator.GetEvent<AfterDetailSavedEvent>().Publish(new
                AfterDetailSavedEventArgs
            {
                Id = modelId,
                DisplayMember = displayMemeber,
                ViewModelName = this.GetType().Name
            });
        }

        protected void SetTitle(string title)
        {
            Title = title;
        }

        protected async virtual void OnCloseDetailViewExecute()
        {
            if (HasChanges)
            {
                var result = await MessageDialogService.ShowOkCancelDialogAsync("You've made changes. Do you still want to close?", "Question");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            EventAggregator.GetEvent<AfterDetailClosedEvent>()
                .Publish(new AfterDetailClosedEventArgs
                {
                    Id = this.Id,
                    ViewModelName = this.GetType().Name
                });
        }

        protected virtual void RaiseCollectionSavedEvent()
        {
            EventAggregator.GetEvent<AfterCollectionSavedEvent>()
                .Publish(new AfterCollectionSavedEventArgs
                {
                    ViewModelName = this.GetType().Name
                });
        }

        protected async Task SaveWithOptimisticConcurrencyAsync(
            Func<Task> saveFunc, 
            Action afterSaveAction)
        {
            try
            {
                await saveFunc();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var databaseValues = ex.Entries.Single().GetDatabaseValues();
                if (databaseValues == null)
                {
                    await MessageDialogService.ShowInfoDialogAsync("The entity has been deleted by another user.");
                    RaiseDetailDeletedEvent(Id);
                    return;
                }

                var result = await MessageDialogService.ShowOkCancelDialogAsync("The entity has been changed in " + "the meantime by someone else. Click OK to save your changes anyway, click Cansel " + "to reload the entity from the database.", "Question");

                if (result == MessageDialogResult.Ok)
                {
                    // Need to update the context I'm using to track the database
                    // with updated values, so the occurency exception stops.
                    var entry = ex.Entries.Single(); // Get my failed entry.
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues()); // Update my entry with the database row version.
                    await saveFunc(); // Save my entity and changes to the database. My updated row version will no longer cause and occurrency exception.
                }
                else
                {
                    // Reload entity from database
                    await ex.Entries.Single().ReloadAsync();
                    await LoadAsync(Id);
                }
            }

            afterSaveAction();
            
        }


    }
}
