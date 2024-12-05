namespace MyQuizApp.App;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		AppDomain.CurrentDomain.UnhandledException += (s, e) =>
		                                              {
			                                              System.Diagnostics.Debug.WriteLine(
				                                              "********** OMG! FirstChanceException **********");
			                                              System.Diagnostics.Debug.WriteLine(e);
		                                              };
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new MainPage()) { Title = "MyQuizApp.App" };

	}
}
