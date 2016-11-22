using System;
using System.Net;
using DespatchBay.addressing.api.despatchbaypro.com;
using DespatchBay.shipping.api.despatchbaypro.com;
using DespatchBay.tracking.api.despatchbaypro.com;

public class DespatchBayApi
{
	string apikey;
	string apiuser;
	string apiendpoint;

	// Constructor
	public DespatchBayApi(string apiuser, string apikey, string apiendpoint = null)
	{
		this.apiuser = apiuser;
		this.apikey = apikey;
		this.apiendpoint = apiendpoint;
	}

/**
	 * Method to return credentials.
	 **/
	public ICredentials getCredentials(string url)
	{
		// Set up some credentials
		var netCredential = new NetworkCredential(apiuser, apikey);
		var uri = new Uri(url);
		ICredentials credentials = netCredential.GetCredential(uri, "Basic");
		return credentials;
	}

	/**
	 * Connects to the api endpoint, authorises and returns an AddressingService Object
	 *  
	**/
	public AddressingService GetAuthoriseService(AddressingService S)
	{
		S.Credentials = getCredentials(S.Url); // credentials;
		return S;
	}

	/**
	 * Connects to the api endpoint, authorises and returns an ShippingService Object
	 *  
	**/
	public ShippingService GetAuthoriseService(ShippingService S)
	{
		S.Credentials = getCredentials(S.Url); // credentials;
		return S;
	}

	/**
	 * Connects to the api endpoint, authorises and returns an trackingService Object
	 *  
	**/
	public TrackingService GetAuthoriseService(TrackingService S)
	{
		S.Credentials = getCredentials(S.Url); // credentials;
		return S;
	}

	/**
	 * Name: GetDomesticAddressKeysByPostcodeMethod
	 * Parameters: String postcode
	 * Returns: Array of objects of type AddressKeyType
	 * Throws an Exception of type SoapEx on failure
	 * 
	**/
	public AddressKeyType[] GetDomesticAddressKeysByPostcodeMethod(string postcode)
	{
		AddressKeyType[] availableAddresses = null;
		try
		{
			var Service = new AddressingService();
			Service = GetAuthoriseService(Service);
			// Call the GetDomesticAddressKeysByPostcode soap service
			availableAddresses = Service.GetDomesticAddressKeysByPostcode(postcode);
		}
		catch (Exception soapEx)
		{
			// Throw the generated exception up.
			throw soapEx;
		}

		return availableAddresses;
	}

	/**
	 * Get a list of available services by postcode
	 **/
	public ServiceType[] GetDomesticServicesByPostcodeMethod(string postcode)
	{
		ServiceType[] availableServices = null;
		try
		{
			var Service = new ShippingService();
			Service = GetAuthoriseService(Service);
			// Call the GetDomesticServicesByPostcode soap service
			availableServices = Service.GetDomesticServicesByPostcode(postcode);
		}
		catch (Exception soapEx)
		{
			// Throw the generated exception up.
			throw soapEx;
		}
		return availableServices;
	}
}