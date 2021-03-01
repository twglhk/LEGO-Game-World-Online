START ../../PacketGenerator/bin/PacketGenerator.exe ../../PacketGenerator/PDL.xml
XCOPY /Y GenPackets.cs "../../DummyClient/Packet"
XCOPY /Y GenPackets.cs "../../../LEGO Game World Online/Assets/Scripts/Network/Packet"
XCOPY /Y GenPackets.cs "../../Server/Packet"

XCOPY /Y PacketManager.cs "../../DummyClient/Packet"
XCOPY /Y PacketManager.cs "../../../LEGO Game World Online/Assets/Scripts/Network/Packet"
XCOPY /Y PacketManager.cs "../../Server/Packet"

pause