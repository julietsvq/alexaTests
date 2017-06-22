using System;
using System.Collections.Generic;
using AlexaSkillsKit.Speechlet;
using AlexaSkillsKit.Slu;
using AlexaSkillsKit.UI;

namespace AlexaTests.Controllers
{
    public class SessionSpeechlet: Speechlet
    {
        private const string NAME_KEY = "name";
        private const string NAME_SLOT = "Name";

        public override SpeechletResponse OnIntent(IntentRequest intentRequest, Session session)
        {
            Intent intent = intentRequest.Intent;
            string intentName = (intent != null) ? intent.Name : null;

            if ("MyNameIsIntent".Equals(intentName))
            {
                return SetNameInSessionAndSayHello(intent, session);
            }

            else if ("WhatsMyNameIntent".Equals(intentName))
            {
                return GetNameFromSessionAndSayHello(intent, session);
            }
            else
            {
                throw new SpeechletException("Invalid Intent");
            }
        }

        public override SpeechletResponse OnLaunch(LaunchRequest launchRequest, Session session)
        {
            return GetWelcomeResponse();
        }

        public override void OnSessionStarted(SessionStartedRequest sessionStartedRequest, Session session)
        {
        }

        public override void OnSessionEnded(SessionEndedRequest sessionEndedRequest, Session session)
        {
        }

        private SpeechletResponse GetWelcomeResponse()
        {
            string speechOutput = "Welcome, what is your name?";
            return BuildSpeechletResponse("Welcome", speechOutput, false);
        }

        private SpeechletResponse SetNameInSessionAndSayHello(Intent intent, Session session)
        {
            Dictionary<string, Slot> slots = intent.Slots;
            Slot nameSlot = slots[NAME_SLOT];
            string speechOutput = "";

            if (nameSlot != null)
            {
                string name = nameSlot.Value;
                session.Attributes[NAME_KEY] = name;
                speechOutput = String.Format("Hello {0}", name);
            }
            else
            {
                speechOutput = "I'm not sure what your name is , please say it again";
            }

            return BuildSpeechletResponse(intent.Name, speechOutput, false);
        }

        private SpeechletResponse GetNameFromSessionAndSayHello(Intent intent, Session session)
        {
            string speechOutput = "";
            bool shouldEndSession = false;

            string name = (String)session.Attributes[NAME_KEY];

            if (!String.IsNullOrEmpty(name))
            {
                speechOutput = String.Format("Your name is {0}, goodbye", name);
                shouldEndSession = true;
            }
            else
            {
                speechOutput = "I'm not sure what your name is";
            }

            return BuildSpeechletResponse(intent.Name, speechOutput, shouldEndSession);
        }

        private SpeechletResponse BuildSpeechletResponse(string title, string speechOutput, bool shouldEndSession)
        {
            SimpleCard card = new SimpleCard();
            card.Title = String.Format("SessionSpeechlet - {0}", title);
            card.Content = String.Format("SessionSpeechlet - {0}", speechOutput);

            PlainTextOutputSpeech speech = new PlainTextOutputSpeech();
            speech.Text = speechOutput;

            SpeechletResponse response = new SpeechletResponse();
            response.ShouldEndSession = shouldEndSession;
            response.OutputSpeech = speech;
            response.Card = card;

            return response;
        }
    }
}