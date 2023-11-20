using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArduinoBluetoothAPI;
using System;
using System.Text;

public class manager : MonoBehaviour
{

	// Use this for initialization
	BluetoothHelper bluetoothHelper;
	BluetoothHelper bluetoothHelper2;
	string deviceName;

	// public Text text;
	public TextMesh text;

	public GameObject sphere;

	public string received_message;

	void Start()
	{
		deviceName = "ESP32-BT"; //bluetooth should be turned ON;
		try
		{
			bluetoothHelper = BluetoothHelper.GetInstance(deviceName);
			bluetoothHelper.OnConnected += OnConnected;
			bluetoothHelper.OnConnectionFailed += OnConnectionFailed;
			bluetoothHelper.OnDataReceived += OnMessageReceived; //read the data
			bluetoothHelper.setTerminatorBasedStream("\n"); //delimits received messages based on \n char

			LinkedList<BluetoothDevice> ds = bluetoothHelper.getPairedDevicesList();

			Debug.Log(ds);

		}
		catch (Exception ex)
		{
			sphere.GetComponent<Renderer>().material.color = Color.yellow;
			Debug.Log(ex.Message);
			text.text = ex.Message;

		}
		StopAllCoroutines();
		StartCoroutine(RepeatedCallConnect());


	}

	IEnumerator blinkSphere()
	{
		sphere.GetComponent<Renderer>().material.color = Color.cyan;
		yield return new WaitForSeconds(0.5f);
		sphere.GetComponent<Renderer>().material.color = Color.green;
	}

	// Update is called once per frame
	void Update()
	{

		/*
		//Synchronous method to receive messages
		if(bluetoothHelper != null)
		if (bluetoothHelper.Available)
			received_message = bluetoothHelper.Read ();
		*/
	}

	//Asynchronous method to receive messages
	void OnMessageReceived()
	{
		//StartCoroutine(blinkSphere());
		received_message = bluetoothHelper.Read();
		Debug.Log(received_message);
		text.text = received_message;
		// Debug.Log(received_message);
	}

	void OnConnected()
	{
		sphere.GetComponent<Renderer>().material.color = Color.green;
		try
		{
			bluetoothHelper.StartListening();

			bluetoothHelper2 = BluetoothHelper.GetNewInstance();
			bluetoothHelper2.OnScanEnded += ScanEnded2;
			bluetoothHelper2.ScanNearbyDevices();
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
		}

	}

	private void ScanEnded2(LinkedList<BluetoothDevice> devices)
	{
		Debug.Log(devices.Count);
	}

	void OnConnectionFailed()
	{
		sphere.GetComponent<Renderer>().material.color = Color.red;
		Debug.Log("Connection Failed");
	}


	//Call this function to emulate message receiving from bluetooth while debugging on your PC.
	/*void OnGUI()
	{
		if (bluetoothHelper != null)
			bluetoothHelper.DrawGUI();
		else
			return;

		if (!bluetoothHelper.isConnected())
			if (GUI.Button(new Rect(Screen.width / 2 - Screen.width / 10, Screen.height / 10, Screen.width / 5, Screen.height / 10), "Connect"))
			{
				if (bluetoothHelper.isDevicePaired())
					bluetoothHelper.Connect(); // tries to connect
				else
					sphere.GetComponent<Renderer>().material.color = Color.magenta;
			}

		if (bluetoothHelper.isConnected())
			if (GUI.Button(new Rect(Screen.width / 2 - Screen.width / 10, Screen.height - 2 * Screen.height / 10, Screen.width / 5, Screen.height / 10), "Disconnect"))
			{
				bluetoothHelper.Disconnect();
				sphere.GetComponent<Renderer>().material.color = Color.blue;
			}

		if (bluetoothHelper.isConnected())
			if (GUI.Button(new Rect(Screen.width / 2 - Screen.width / 10, Screen.height / 10, Screen.width / 5, Screen.height / 10), "Send text"))
			{
				bluetoothHelper.SendData(new Byte[] { 0, 0, 85, 0, 85 });
				// bluetoothHelper.SendData("This is a very long long long long text");
			}
	}*/

	void OnDestroy()
	{
		if (bluetoothHelper != null)
			bluetoothHelper.Disconnect();
		StopAllCoroutines();
	}

	void ConnectESP()
	{
		if (!bluetoothHelper.isConnected())
			if (bluetoothHelper.isDevicePaired())
				bluetoothHelper.Connect(); // tries to connect
			else
				sphere.GetComponent<Renderer>().material.color = Color.magenta;


	}

	IEnumerator RepeatedCallConnect()
	{
		while (true)
		{
			// Call your function here
			ConnectESP();

			// Wait for three seconds
			yield return new WaitForSeconds(3.0f);
		}
	}
}
