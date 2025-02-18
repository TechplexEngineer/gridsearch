using System;
using System.Collections.Generic;
using System.Text;

namespace libsecondlife.Voice
{
    public partial class VoiceGateway
    {
        /// <summary>
        /// Create a Session
        /// Sessions typically represent a connection to a media session with one or more
        /// participants. This is used to generate an �outbound� call to another user or
        /// channel. The specifics depend on the media types involved. A session handle is
        /// required to control the local user functions within the session (or remote
        /// users if the current account has rights to do so). Currently creating a
        /// session automatically connects to the audio media, there is no need to call
        /// Session.Connect at this time, this is reserved for future use.
        /// </summary>
        /// <param name="ConnectorHandle">Handle returned from successful Connector �create� request</param>
        /// <param name="URI">This is the URI of the terminating point of the session (ie who/what is being called)</param>
        /// <param name="Name">This is the display name of the entity being called (user or channel)</param>
        /// <param name="Password">Only needs to be supplied when the target URI is password protected</param>
        /// <param name="PasswordHashAlgorithm">This indicates the format of the password as passed in. This can either be
        /// �ClearText� or �SHA1UserName�. If this element does not exist, it is assumed to be �ClearText�. If it is
        /// �SHA1UserName�, the password as passed in is the SHA1 hash of the password and username concatenated together,
        /// then base64 encoded, with the final �=� character stripped off.</param>
        /// <returns></returns>
        public int SessionCreate(string AccountHandle, string URI, string Name, string Password, bool JoinAudio, bool JoinText, string PasswordHashAlgorithm)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(VoiceGateway.MakeXML("AccountHandle", AccountHandle));
            sb.Append(VoiceGateway.MakeXML("URI", URI));
            sb.Append(VoiceGateway.MakeXML("Name", Name));
            sb.Append(VoiceGateway.MakeXML("Password", Password));
            sb.Append(VoiceGateway.MakeXML("JoinAudio", JoinAudio ? "true" : "false"));
            sb.Append(VoiceGateway.MakeXML("JoinText", JoinText ? "true" : "false"));
            sb.Append(VoiceGateway.MakeXML("PasswordHashAlgorithm", PasswordHashAlgorithm));
            return Request("Session.Create.1", sb.ToString());
        }

        /// <summary>
        /// Used to accept a call
        /// </summary>
        /// <param name="SessionHandle">SessionHandle such as received from SessionNewEvent</param>
        /// <param name="AudioMedia">"default"</param>
        /// <returns></returns>
        public int SessionConnect(string SessionHandle, string AudioMedia)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(VoiceGateway.MakeXML("SessionHandle", SessionHandle));
            sb.Append(VoiceGateway.MakeXML("AudioMedia", AudioMedia));
            return Request("Session.Connect.1", sb.ToString());
        }

        /// <summary>
        /// This command is used to start the audio render process, which will then play
        /// the passed in file through the selected audio render device. This command
        /// should not be issued if the user is on a call.
        /// </summary>
        /// <param name="SoundFilePath">The fully qualified path to the sound file.</param>
        /// <param name="Loop">True if the file is to be played continuously and false if it is should be played once.</param>
        /// <returns></returns>
        public int SessionRenderAudioStart(string SoundFilePath, bool Loop)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(VoiceGateway.MakeXML("SoundFilePath", SoundFilePath));
            sb.Append(VoiceGateway.MakeXML("Loop", Loop ? "1" : "0"));
            return Request("Session.RenderAudioStart.1", sb.ToString());
        }

        /// <summary>
        /// This command is used to stop the audio render process.
        /// </summary>
        /// <param name="SoundFilePath">The fully qualified path to the sound file issued in the start render command.</param>
        /// <returns></returns>
        public int SessionRenderAudioStop(string SoundFilePath)
        {
            string RequestXML = VoiceGateway.MakeXML("SoundFilePath", SoundFilePath);
            return Request("Session.RenderAudioStop.1", RequestXML);
        }

        /// <summary>
        /// This is used to �end� an established session (i.e. hang-up or disconnect).
        /// </summary>
        /// <param name="SessionHandle">Handle returned from successful Session �create� request or a SessionNewEvent</param>
        /// <returns></returns>
        public int SessionTerminate(string SessionHandle)
        {
            string RequestXML = VoiceGateway.MakeXML("SessionHandle", SessionHandle);
            return Request("Session.Terminate.1", RequestXML);
        }

        /// <summary>
        /// Set the combined speaking and listening position in 3D space.
        /// There appears to be no response to this request.
        /// </summary>
        /// <param name="SessionHandle">Handle returned from successful Session �create� request or a SessionNewEvent</param>
        /// <param name="SpeakerPosition">Speaking position</param>
        /// <param name="ListenerPosition">Listening position</param>
        /// <returns></returns>
        public int SessionSet3DPosition(string SessionHandle, VoicePosition SpeakerPosition, VoicePosition ListenerPosition)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(VoiceGateway.MakeXML("SessionHandle", SessionHandle));
            sb.Append("<SpeakerPosition>");
            sb.Append("<Position>");
            sb.Append(VoiceGateway.MakeXML("X", SpeakerPosition.Position.X.ToString()));
            sb.Append(VoiceGateway.MakeXML("Y", SpeakerPosition.Position.Y.ToString()));
            sb.Append(VoiceGateway.MakeXML("Z", SpeakerPosition.Position.Z.ToString()));
            sb.Append("</Position>");
            sb.Append("<Velocity>");
            sb.Append(VoiceGateway.MakeXML("X", SpeakerPosition.Velocity.X.ToString()));
            sb.Append(VoiceGateway.MakeXML("Y", SpeakerPosition.Velocity.Y.ToString()));
            sb.Append(VoiceGateway.MakeXML("Z", SpeakerPosition.Velocity.Z.ToString()));
            sb.Append("</Velocity>");
            sb.Append("<AtOrientation>");
            sb.Append(VoiceGateway.MakeXML("X", SpeakerPosition.AtOrientation.X.ToString()));
            sb.Append(VoiceGateway.MakeXML("Y", SpeakerPosition.AtOrientation.Y.ToString()));
            sb.Append(VoiceGateway.MakeXML("Z", SpeakerPosition.AtOrientation.Z.ToString()));
            sb.Append("</AtOrientation>");
            sb.Append("<UpOrientation>");
            sb.Append(VoiceGateway.MakeXML("X", SpeakerPosition.UpOrientation.X.ToString()));
            sb.Append(VoiceGateway.MakeXML("Y", SpeakerPosition.UpOrientation.Y.ToString()));
            sb.Append(VoiceGateway.MakeXML("Z", SpeakerPosition.UpOrientation.Z.ToString()));
            sb.Append("</UpOrientation>");
            sb.Append("<LeftOrientation>");
            sb.Append(VoiceGateway.MakeXML("X", SpeakerPosition.LeftOrientation.X.ToString()));
            sb.Append(VoiceGateway.MakeXML("Y", SpeakerPosition.LeftOrientation.Y.ToString()));
            sb.Append(VoiceGateway.MakeXML("Z", SpeakerPosition.LeftOrientation.Z.ToString()));
            sb.Append("</LeftOrientation>");
            sb.Append("</SpeakerPosition>");
            sb.Append("<ListenerPosition>");
            sb.Append("<Position>");
            sb.Append(VoiceGateway.MakeXML("X", ListenerPosition.Position.X.ToString()));
            sb.Append(VoiceGateway.MakeXML("Y", ListenerPosition.Position.Y.ToString()));
            sb.Append(VoiceGateway.MakeXML("Z", ListenerPosition.Position.Z.ToString()));
            sb.Append("</Position>");
            sb.Append("<Velocity>");
            sb.Append(VoiceGateway.MakeXML("X", ListenerPosition.Velocity.X.ToString()));
            sb.Append(VoiceGateway.MakeXML("Y", ListenerPosition.Velocity.Y.ToString()));
            sb.Append(VoiceGateway.MakeXML("Z", ListenerPosition.Velocity.Z.ToString()));
            sb.Append("</Velocity>");
            sb.Append("<AtOrientation>");
            sb.Append(VoiceGateway.MakeXML("X", ListenerPosition.AtOrientation.X.ToString()));
            sb.Append(VoiceGateway.MakeXML("Y", ListenerPosition.AtOrientation.Y.ToString()));
            sb.Append(VoiceGateway.MakeXML("Z", ListenerPosition.AtOrientation.Z.ToString()));
            sb.Append("</AtOrientation>");
            sb.Append("<UpOrientation>");
            sb.Append(VoiceGateway.MakeXML("X", ListenerPosition.UpOrientation.X.ToString()));
            sb.Append(VoiceGateway.MakeXML("Y", ListenerPosition.UpOrientation.Y.ToString()));
            sb.Append(VoiceGateway.MakeXML("Z", ListenerPosition.UpOrientation.Z.ToString()));
            sb.Append("</UpOrientation>");
            sb.Append("<LeftOrientation>");
            sb.Append(VoiceGateway.MakeXML("X", ListenerPosition.LeftOrientation.X.ToString()));
            sb.Append(VoiceGateway.MakeXML("Y", ListenerPosition.LeftOrientation.Y.ToString()));
            sb.Append(VoiceGateway.MakeXML("Z", ListenerPosition.LeftOrientation.Z.ToString()));
            sb.Append("</LeftOrientation>");
            sb.Append("</ListenerPosition>");
            return Request("Session.Set3DPosition.1", sb.ToString());
        }

        /// <summary>
        /// Set User Volume for a particular user. Does not affect how other users hear that user.
        /// </summary>
        /// <param name="SessionHandle">Handle returned from successful Session �create� request or a SessionNewEvent</param>
        /// <param name="ParticipantURI"></param>
        /// <param name="Volume">The level of the audio, a number between -100 and 100 where 0 represents �normal� speaking volume</param>
        /// <returns></returns>
        public int SessionSetParticipantVolumeForMe(string SessionHandle, string ParticipantURI, int Volume)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(VoiceGateway.MakeXML("SessionHandle", SessionHandle));
            sb.Append(VoiceGateway.MakeXML("ParticipantURI", ParticipantURI));
            sb.Append(VoiceGateway.MakeXML("Volume", Volume.ToString()));
            return Request("Session.SetParticipantVolumeForMe.1", sb.ToString());
        }
    }
}
