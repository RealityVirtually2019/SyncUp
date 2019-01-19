using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotonAvatarView : MonoBehaviour, IPunObservable {

    private PhotonView photonView;
    private OvrAvatar ovrAvatar;
    private OvrAvatarRemoteDriver remoteDriver;
    private int localSequence;

    private List<byte[]> packetData;

	// Use this for initialization
	void Start () {

        photonView = GetComponent<PhotonView>();

        if(photonView.IsMine)
        {
            ovrAvatar = GetComponent<OvrAvatar>();
            ovrAvatar.RecordPackets = true;
            ovrAvatar.PacketRecorded += OnLocalAvatarPacketRecorded;

            packetData = new List<byte[]>();
        }
        else
        {
            remoteDriver = GetComponent<OvrAvatarRemoteDriver>();
        }
        
	}
    /*
    After getting the reference to our PhotonView component we can directly use
    its isMine condition in order to have a clear cut between the 'Local' and the 'Remote Avatar'.
        If the instantiated object is ours we get the reference to the OvrAvatar component, 
        set an event handler which is fired when a new packet is recorded and also instantiate
        the list of byte-arrays which stores all Avatar related input events before sending this
        data across the network.If the object belongs to another client we get the reference to the OvrAvatarRemoteDriver component which is
        later used to imitate our input so that other clients see our gesture.

	*/

    //use to stop recording packets which contain our gesture.
    public void OnDisable()
    {
        if (photonView.IsMine)
        {
            ovrAvatar.RecordPackets = false;
            ovrAvatar.PacketRecorded -= OnLocalAvatarPacketRecorded;
        }
    }

    /*
     * In the next step this packet gets serialized into a byte-array which is
     * supported by PUN out of the box. Afterwards it is added to our previously
     * created list and is then ready to get sent across the network. In order to
     * avoid sending unnecessary data and prevent a disconnect, that might occur
     * due to message size exceeding, we implement a condition first which checks
     * if we have to process the other part of the function at all.
     * 
     */

    public void OnLocalAvatarPacketRecorded(object sender, OvrAvatar.PacketEventArgs args)
    {
        if (!PhotonNetwork.InRoom || (PhotonNetwork.CurrentRoom.PlayerCount < 2))
        {
            return;
        }

        using (MemoryStream outputStream = new MemoryStream())
        {
            BinaryWriter writer = new BinaryWriter(outputStream);

            var size = Oculus.Avatar.CAPI.ovrAvatarPacket_GetSize(args.Packet.ovrNativePacket);
            byte[] data = new byte[size];
            Oculus.Avatar.CAPI.ovrAvatarPacket_Write(args.Packet.ovrNativePacket, size, data);

            writer.Write(localSequence++);
            writer.Write(size);
            writer.Write(data);

            packetData.Add(outputStream.ToArray());
        }
    }

    /*
     * Since we have a serializer now we also need a deserializer which helps us deserializing
     * the received packets. Thus our next task is to implement this function. 
     * 
     * 
     */



    private void DeserializeAndQueuePacketData(byte[] data)
    {
        using (MemoryStream inputStream = new MemoryStream(data))
        {
            BinaryReader reader = new BinaryReader(inputStream);
            int remoteSequence = reader.ReadInt32();

            int size = reader.ReadInt32();
            byte[] sdkData = reader.ReadBytes(size);

            System.IntPtr packet = Oculus.Avatar.CAPI.ovrAvatarPacket_Read((System.UInt32)data.Length, sdkData);
            remoteDriver.QueuePacket(remoteSequence, new OvrAvatarPacket { ovrNativePacket = packet });
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //executed by the owner of the game object
        if (stream.IsWriting)
        {
            if (packetData.Count == 0)
            {
                return;
            }

            stream.SendNext(packetData.Count);

            foreach (byte[] b in packetData)
            {
                stream.SendNext(b);
            }

            packetData.Clear();
        }

        //is only excuted on the remote clients, those who dont,
        //own the object. It firstly checks how many packets we have to process
        //and then calls our previous implemented function to deserialize and queue all packet data
        //step by step.

        if (stream.IsReading)
        {
            int num = (int)stream.ReceiveNext();

            for (int counter = 0; counter < num; ++counter)
            {
                byte[] data = (byte[])stream.ReceiveNext();

                DeserializeAndQueuePacketData(data);
            }
        }
    }


    // Update is called once per frame
    void Update () {
		
	}
}
