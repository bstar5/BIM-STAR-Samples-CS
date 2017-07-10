using System.Windows.Media;
using Mango;

namespace DemoApp.ViewModels
{
    public class FirstViewModel : ViewModelBase
    {
        public FirstViewModel()
        {
            ImageSource = this.GetAppImageSource("Assets\\测试图片.png");
            ImageUrl = "https://ss0.bdstatic.com/70cFvHSh_Q1YnxGkpoWK1HF6hhy/it/u=2300258792,2484561302&fm=23&gp=0.jpg";
        }

        private ImageSource _imageSource;

        /// <summary>
        /// 获取或设置ImageSource属性
        /// </summary>
        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set { Set("ImageSource", ref _imageSource, value); }
        }

        private string _imageUrl;

        /// <summary>
        /// 获取或设置ImageUrl属性
        /// </summary>
        public string ImageUrl
        {
            get { return _imageUrl; }
            set { Set("ImageUrl", ref _imageUrl, value); }
        }
    }
}