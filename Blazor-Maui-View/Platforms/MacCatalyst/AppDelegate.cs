using Foundation;

namespace Blazor_Maui_View {
	[Register("AppDelegate")]
	public class AppDelegate : MauiUIApplicationDelegate {
		protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
	}
}