namespace Opdracht_8
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnNavigateToVragenPaginaClicked(object sender, EventArgs e)
        {
            Console.WriteLine("Button clicked!");
            Navigation.PushAsync(new VragenPagina());
        }

    }
}
