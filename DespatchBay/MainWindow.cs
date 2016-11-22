using System;
using Gtk;
using DespatchBay.addressing.api.despatchbaypro.com;
using DespatchBay.shipping.api.despatchbaypro.com;

public partial class MainWindow : Window
{

	// TODO: needs moving to configuration and paramatising to DEBUG and RELEASE Environments
	static string apiuser = "26JG-C6HWIO51";
	static string apikey = "YI8JLR9";



	// a simple alert box method
	static void MessageBox(string message = "Message not set yet")
	{
		var md = new MessageDialog(null, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, message);
		md.Run();
		md.Destroy();
	}

	public MainWindow() : base(WindowType.Toplevel)
	{
#pragma warning disable RECS0021 // Warns about calls to virtual member functions occuring in the constructor
		Build();
#pragma warning restore RECS0021 // Warns about calls to virtual member functions occuring in the constructor
	}

	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
	}

	// Exit application method
	protected void OnExit(object sender, EventArgs e)
	{
		Application.Quit();
	}

	// look up postcode
	protected void OnLookUP(object sender, EventArgs e)
	{
		string strPostcode = entry1.Text;

		if (strPostcode.Length == 0)
		{
			MessageBox("Postcode field is Empty");
			return;
		}

		getAddresses(strPostcode);
		getServices(strPostcode);
	}

	void getAddresses(string postcode)
	{
		AddressKeyType[] availableAddresses = null;
		try
		{
			var addressingRequest = new DespatchBayApi(apiuser, apikey);
			availableAddresses = addressingRequest.GetDomesticAddressKeysByPostcodeMethod(postcode);

			if (availableAddresses != null)
			{
				foreach (AddressKeyType element in availableAddresses)
				{
					addressListBox.AppendText(element.Address);
				}
			}
			else {
				MessageBox("No addresses found for postcode " + postcode);
			}
		}
		catch (Exception)
		{
			MessageBox("An error occoured");
		}
	}


	private void getServices(string postcode)
	{
		ServiceType[] availableServices = null;

		// Define a storeage objecy to hold the retuened services
		var store = new ListStore(typeof(string), typeof(int), typeof(float));
		try
		{
			var shippingRequest = new DespatchBayApi(apiuser, apikey);
			availableServices = shippingRequest.GetDomesticServicesByPostcodeMethod(postcode);
			if (availableServices != null)
			{
				foreach (var element in availableServices)
				{
					store.AppendValues(element.Name + " (£"+ element.Cost + ")", element.ServiceID);
				}
				// Applu the data model to the listbox object
				servicesListBox.Model = store;
			}
			else {
				MessageBox("No services found for postcode " + postcode);
			}

		}
		catch (Exception)
		{
			MessageBox("An error occoured");
		}

	}

	protected void SelectService(object sender, EventArgs e)
	{

	}

	protected void onBook(object sender, EventArgs e)
	{
		if (this.addressListBox.ActiveText == null)
		{
			MessageBox("You must select an address");
		}
	}
}

