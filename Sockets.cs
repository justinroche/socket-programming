var ip = "127.0.0.1";
var port = 11000;

void Client() {
	var sender = create a socket;
	connect sender to ip and port;

	var msg1 = text from user;

	print("Sending " + msg1);

	var bytes = encode msg from string to byte[];

	sender.Send(bytes);

	print("Waiting for response...");

	var response = sender.Receive();

	var msg2 = decode response from byte[] to string;

	print("Received " + msg2);

	shutdown and close sender;
}

void Server() {
	var listener = create a socket;
	bind listener to ip and port;

	for(; ;){
		var handler = listener.Accept();

		var bytes1 = handler.Receive();

		var request = decode bytes1 from byte[] to string;

		print("Received " + request);

		var response = request.ToUpperCase();

		var bytes2 = encode response from string to byte[];

		print("Sending " + response);

		handler.Send(bytes2);
		
		shutdown and close handler;
	}

	shutdown and close listener;
}