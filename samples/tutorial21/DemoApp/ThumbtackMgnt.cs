using System;
using System.Collections.Generic;
using DemoApp.Models;
using Mango;

namespace DemoApp
{
    internal class ThumbtackMgnt : IDisposable
    {
        private static dynamic _scope;
        public List<ThumbtackInfo> ThumbtackInfoList;

        public ThumbtackMgnt()
        {
            ThumbtackInfoList = new List<ThumbtackInfo>();
        }

        private static void EnsureScropeCreated()
        {
            if (_scope != null)
                return;
            var filePath = typeof(ThumbtackMgnt).GetAppResPath("Scripts/Thumbtack.py");
            var result = Mg.Get<IMgService>().Invoke<DemoApp>("GraphicPlatform:CreateScriptScope", new
            {
                FilePath = filePath,
                typeof(ThumbtackMgnt).Assembly
            });
            if (!result.IsOk)
            {
                Mg.Get<IMgDialog>().ShowDesktopAlert("提示", result.Message);
                return;
            }
            _scope = result.Data;
        }

        public void Dispose()
        {
            ThumbtackInfoList.Clear();
            ThumbtackInfoList = null;
        }

        public void ShowThumbtack(ThumbtackInfo ttInfo)
        {
            EnsureScropeCreated();
            if (_scope == null)
                return;
            var action = _scope.GetVariable<Action<ThumbtackInfo>>("ShowThumbtack");
            if (action == null)
                return;
            action(ttInfo);
        }

        public void RemoveThumbtack(ThumbtackInfo ttInfo)
        {
            if (_scope == null)
                return;
            var action = _scope.GetVariable<Action<string>>("RemoveThumbtack");
            if (action == null)
                return;
            action(ttInfo.Key);
        }
    }
}