﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using xf.simp.news.Model;
using xf.simp.news.Services;

namespace xf.simp.news.ViewModels
{
    public class ReportsViewModel : NotificationEnabledObject
    {
        public ReportsViewModel()
        {
            Inicialize();
        }

        private void Inicialize()
        {
            MxMService = new UniversalMxMService();
            MxMService.GetNews_Completed += (s, a) =>
            {
                Reports = new ObservableCollection<Report>(a.Result);
                IsBusy = false;
            };
            MxMService.GetNews();

            UpdateArticles = new ActionCommand<string>((param) =>
            {
                IsBusy = true;
                MxMService.GetNews();
            });
        }

        private UniversalMxMService _MxMService;
        public UniversalMxMService MxMService
        {
            get => _MxMService;
            set => Set(ref _MxMService, value);
        }

        private ObservableCollection<Report> _Reports;
        public ObservableCollection<Report> Reports
        {
            get => _Reports;
            set => Set(ref _Reports, value);
        }

        private bool _IsBusy;
        public bool IsBusy
        {
            get => _IsBusy;
            set => Set(ref _IsBusy, value);
        }

        private ActionCommand<Report> _ShowArticle_Command;
        public ActionCommand<Report> ShowArticle_Command
        {
            get
            {
                if (_ShowArticle_Command == null)
                {
                    _ShowArticle_Command = new ActionCommand<Report>((report) =>
                   {
                       Browser.OpenAsync(report.Link, BrowserLaunchMode.SystemPreferred);
                   });
                }
                return _ShowArticle_Command;
            }
            set
            {
                _ShowArticle_Command = value;
                OnPropertyChanged();
            }
        }

        public ActionCommand<string> UpdateArticles { private set; get; }
    }
}