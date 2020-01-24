using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

public class WebSocket
{
	private Uri url;
	private WebSocketSharp.WebSocket socket;
	private Queue <byte []> messages = new Queue <byte []> ();
	private bool isConnected = false;
	private string error = null;

	public WebSocket (Uri url)
	{
		this.url = url;

		string protocol = this.url.Scheme;

		if (!protocol.Equals ("ws") && !protocol.Equals ("wss"))
			throw new ArgumentException ("Unsupported protocol: " + protocol);

        Connect();
	}

	public void SendString (string message)
	{
		socket.Send (message);
	}

	public string ReceiveString ()
	{
		byte [] retval = Receive ();

		if (retval == null)
			return null;

		return Encoding.UTF8.GetString (retval);
	}

	private byte [] Receive ()
	{
		if (messages.Count == 0)
			return null;

		return messages.Dequeue ();
	}

	public IEnumerator Connect ()
	{
		socket = new WebSocketSharp.WebSocket (url.ToString ());
		socket.OnMessage += (sender, e) => messages.Enqueue (e.RawData);
		socket.OnOpen += (sender, e) => isConnected = true;
		socket.OnError += (sender, e) => error = e.Message;
		socket.ConnectAsync ();

		while (!isConnected && error == null)
			yield return 0;
	}

	public void Close ()
	{
		socket.Close ();
	}

	public string GetError ()
	{
		return error;
	}
}