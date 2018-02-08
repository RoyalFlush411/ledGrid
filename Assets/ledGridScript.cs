using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using ledGrid;

public class ledGridScript : MonoBehaviour
{
		public KMBombInfo Bomb;
		public KMSelectable aButton;
		public KMSelectable bButton;
		public KMSelectable cButton;
		public KMSelectable dButton;

		public KMAudio Audio;

		//Light renderers & textures
		public List <Renderer> lights;
		public List <Texture> colours;

		//Vital Information
		private List <String> selectedColours = new List <string>();
		string input;
		string correctAnswer;
		int blackCount = 0;

		//Logging
		static int moduleIdCounter = 1;
		int moduleId;
		private string TwitchHelpMessage = "Type '!{0} press abcd'";

		public KMSelectable[] ProcessTwitchCommand(string command)
		{
				if (command.Equals("press abcd", StringComparison.InvariantCultureIgnoreCase))
				{
						return new KMSelectable[] { aButton, bButton, cButton, dButton };
				}
				else if (command.Equals("press abdc", StringComparison.InvariantCultureIgnoreCase))
				{
						return new KMSelectable[] { aButton, bButton, dButton, cButton };
				}
				else if (command.Equals("press acbd", StringComparison.InvariantCultureIgnoreCase))
				{
						return new KMSelectable[] { aButton, cButton, bButton, dButton };
				}
				else if (command.Equals("press acdb", StringComparison.InvariantCultureIgnoreCase))
				{
						return new KMSelectable[] { aButton, cButton, dButton, bButton };
				}
				else if (command.Equals("press adbc", StringComparison.InvariantCultureIgnoreCase))
				{
						return new KMSelectable[] { aButton, dButton, bButton, cButton };
				}
				else if (command.Equals("press adcb", StringComparison.InvariantCultureIgnoreCase))
				{
						return new KMSelectable[] { aButton, dButton, cButton, bButton };
				}
				else if (command.Equals("press bacd", StringComparison.InvariantCultureIgnoreCase))
				{
						return new KMSelectable[] { bButton, aButton, cButton, dButton };
				}
				else if (command.Equals("press badc", StringComparison.InvariantCultureIgnoreCase))
				{
						return new KMSelectable[] { bButton, aButton, dButton, cButton };
				}
				else if (command.Equals("press bcad", StringComparison.InvariantCultureIgnoreCase))
				{
						return new KMSelectable[] { bButton, cButton, aButton, dButton };
				}
				else if (command.Equals("press bcda", StringComparison.InvariantCultureIgnoreCase))
				{
						return new KMSelectable[] { bButton, cButton, dButton, aButton };
				}
				else if (command.Equals("press bdac", StringComparison.InvariantCultureIgnoreCase))
				{
						return new KMSelectable[] { bButton, dButton, aButton, cButton };
				}
				else if (command.Equals("press bdca", StringComparison.InvariantCultureIgnoreCase))
				{
						return new KMSelectable[] { bButton, dButton, cButton, aButton };
				}
				else if (command.Equals("press cabd", StringComparison.InvariantCultureIgnoreCase))
				{
						return new KMSelectable[] { cButton, aButton, bButton, dButton };
				}
				else if (command.Equals("press cadb", StringComparison.InvariantCultureIgnoreCase))
				{
						return new KMSelectable[] { cButton, aButton, dButton, bButton };
				}
				else if (command.Equals("press cbad", StringComparison.InvariantCultureIgnoreCase))
				{
						return new KMSelectable[] { cButton, bButton, aButton, dButton };
				}
				else if (command.Equals("press cbda", StringComparison.InvariantCultureIgnoreCase))
				{
						return new KMSelectable[] { cButton, bButton, dButton, aButton };
				}
				else if (command.Equals("press cdab", StringComparison.InvariantCultureIgnoreCase))
				{
						return new KMSelectable[] { cButton, dButton, aButton, bButton };
				}
				else if (command.Equals("press cdba", StringComparison.InvariantCultureIgnoreCase))
				{
						return new KMSelectable[] { cButton, dButton, bButton, aButton };
				}
				else if (command.Equals("press dabc", StringComparison.InvariantCultureIgnoreCase))
				{
						return new KMSelectable[] { dButton, aButton, bButton, cButton };
				}
				else if (command.Equals("press dacb", StringComparison.InvariantCultureIgnoreCase))
				{
						return new KMSelectable[] { dButton, aButton, cButton, dButton };
				}
				else if (command.Equals("press dbac", StringComparison.InvariantCultureIgnoreCase))
				{
						return new KMSelectable[] { dButton, bButton, aButton, cButton };
				}
				else if (command.Equals("press dbca", StringComparison.InvariantCultureIgnoreCase))
				{
						return new KMSelectable[] { dButton, bButton, cButton, aButton };
				}
				else if (command.Equals("press dcab", StringComparison.InvariantCultureIgnoreCase))
				{
						return new KMSelectable[] { dButton, cButton, aButton, bButton };
				}
				else if (command.Equals("press dcba", StringComparison.InvariantCultureIgnoreCase))
				{
						return new KMSelectable[] { dButton, cButton, bButton, aButton };
				}
				return null;
		}

		void Awake ()
		{
				moduleId = moduleIdCounter++;
				aButton.OnInteract += delegate () { OnaButton(); return false; };
				bButton.OnInteract += delegate () { OnbButton(); return false; };
				cButton.OnInteract += delegate () { OncButton(); return false; };
				dButton.OnInteract += delegate () { OndButton(); return false; };
		}

		void Start ()
		{
				SetupLights();
				Debug.LogFormat("[LED Grid #{0}] The chosen LED colours are {1}.", moduleId, string.Join(", ", lights.Select((x) => x.material.mainTexture.name).ToArray()));
				AnswerChecker();
		}

		void SetupLights()
		{
		    foreach (Renderer lightRenderer in lights)
		    {
		        int colourIndex = 0;
		        if (blackCount == 4)
		        {
		            colourIndex = UnityEngine.Random.Range(0, colours.Count - 3);
		        }
		        else
		        {
		            colourIndex = UnityEngine.Random.Range(0, colours.Count);
		        }

		        if (colourIndex >= colours.Count - 3)
		        {
		            blackCount++;
		        }
		        lightRenderer.material.mainTexture = colours[colourIndex];
						selectedColours.Add(lightRenderer.material.mainTexture.name);
		    }
		}

		void AnswerChecker()
		{
				if (selectedColours.Where((x) => x.Equals("black")).Count() == 4) //4 lights off
	 			{
						Debug.LogFormat("[LED Grid #{0}] There are 4 LEDs off.", moduleId);
						if (selectedColours.Skip(3).Take(3).Distinct().Count() == 1)
						{
								correctAnswer = "BCDA";
						}
						else if (selectedColours.Where((x) => x.Equals("green")).Count() >= 2)
				    {
								correctAnswer = "ABDC";
				    }
						else if (selectedColours.GroupBy((x) => x).Where((y) => y.Count() == 2).Count() == 2)
						{
								correctAnswer = "CBDA";
						}
						else if (!selectedColours.Any((x) => x.Equals("pink")))
				    {
								correctAnswer = "DABC";
				    }
						else
						{
								correctAnswer = "ABCD";
						}
	 			}
				else if (selectedColours.Where((x) => x.Equals("black")).Count() == 3) //3 lights off
				{
						Debug.LogFormat("[LED Grid #{0}] There are 3 LEDs off.", moduleId);
						if (selectedColours.Where((x) => x.Equals("orange")).Count() == 2)
						{
								correctAnswer = "BDAC";
						}
						else if (selectedColours.GroupBy((x) => x).Where((y) => y.Count() == 2).Count() > 1)
						{
								correctAnswer = "CABD";
						}
						else if (!selectedColours.Any((x) => x.Equals("purple")))
						{
								correctAnswer = "DCAB";
						}
						else if ((selectedColours.Where((x) => x.Equals("red")).Count() >= 1) && (selectedColours.Where((x) => x.Equals("yellow")).Count() >= 1))
						{
								correctAnswer = "ACBD";
						}
						else
						{
								correctAnswer = "BDCA";
						}
				}
				else if (selectedColours.Where((x) => x.Equals("black")).Count() == 2) //2 lights off
				{
						Debug.LogFormat("[LED Grid #{0}] There are 2 LEDs off.", moduleId);
						if (selectedColours.Where((x) => x.Equals("purple")).Count() >= 3)
						{
								correctAnswer = "ADCB";
						}
						else if (selectedColours.GroupBy((x) => x).Where((y) => y.Count() == 2).Count() == 2)
						{
								correctAnswer = "BCAD";
						}
						else if ((selectedColours.Where((x) => x.Equals("white")).Count() >= 1) && (selectedColours.Where((x) => x.Equals("orange")).Count() >= 1) && (selectedColours.Where((x) => x.Equals("pink")).Count() >= 1))
						{
								correctAnswer = "DBCA";
						}
						else if ((selectedColours.Where((x) => x.Equals("blue")).Count() == 4) || (selectedColours.Where((x) => x.Equals("red")).Count() == 3) || (selectedColours.Where((x) => x.Equals("yellow")).Count() == 2) || (selectedColours.Where((x) => x.Equals("green")).Count() == 1))
						{
								correctAnswer = "CADB";
						}
						else
						{
								correctAnswer = "CDBA";
						}
				}
				else if (selectedColours.Where((x) => x.Equals("black")).Count() == 1) //1 light off
				{
						Debug.LogFormat("[LED Grid #{0}] There is 1 LED off.", moduleId);
						if (selectedColours.Distinct().Count() == 9)
						{
								correctAnswer = "DCBA";
						}
						else if (selectedColours.Take(3).Distinct().Count() == 1)
						{
								correctAnswer = "ADBC";
						}
						else if ((selectedColours.Where((x) => x.Equals("red")).Count() == 3) || (selectedColours.Where((x) => x.Equals("pink")).Count() == 3) || (selectedColours.Where((x) => x.Equals("purple")).Count() == 3))
						{
								correctAnswer = "CBAD";
						}
						else if ((selectedColours.Where((x) => x.Equals("white")).Count() == 1) || (selectedColours.Where((x) => x.Equals("blue")).Count() == 2) || (selectedColours.Where((x) => x.Equals("yellow")).Count() == 3))
						{
								correctAnswer = "BADC";
						}
						else
						{
								correctAnswer = "DBAC";
						}
				}
				else if (selectedColours.Where((x) => x.Equals("black")).Count() < 1) //0 light off
				{
						Debug.LogFormat("[LED Grid #{0}] There are 0 LEDs off.", moduleId);
						if (!selectedColours.Any((x) => x.Equals("orange")))
						{
								correctAnswer = "CDAB";
						}
						else if (selectedColours.Where((x) => x.Equals("red")).Count() >= 3)
						{
								correctAnswer = "DACB";
						}
						else if (selectedColours.GroupBy((x) => x).Where((y) => y.Count() == 2).Count() >= 2)
						{
								correctAnswer = "BACD";
						}
						else if (selectedColours.Skip(6).Take(3).Distinct().Count() == 1)
						{
								correctAnswer = "ACDB";
						}
						else
						{
								correctAnswer = "BCDA";
						}
				}
				Debug.LogFormat("[LED Grid #{0}] The correct button sequence is {1}.", moduleId, correctAnswer);
		}

		void InputChecker()
		{
				if (input.Length < 4)
				{
						return;
				}
				else if (input == correctAnswer)
				{
						Debug.LogFormat("[LED Grid #{0}] You pressed {1}. That is correct. Module disarmed.", moduleId, correctAnswer);
						GetComponent<KMBombModule>().HandlePass();
						Audio.PlaySoundAtTransform("passSound", transform);
				}
				else
				{
						GetComponent<KMBombModule>().HandleStrike();
						Debug.LogFormat("[LED Grid #{0}] Strike! You pressed {1}. I was expecting {2}.", moduleId, input, correctAnswer);
						input = "";
						Audio.PlaySoundAtTransform("strikeSound", transform);
				}
		}

		//Buttons
		public void OnaButton()
		{
				GetComponent<KMSelectable>().AddInteractionPunch();
				Audio.PlaySoundAtTransform("aSound", transform);
				input += "A";
				InputChecker();
		}
		public void OnbButton()
		{
				GetComponent<KMSelectable>().AddInteractionPunch();
				Audio.PlaySoundAtTransform("bSound", transform);
				input += "B";
				InputChecker();
		}
		public void OncButton()
		{
				GetComponent<KMSelectable>().AddInteractionPunch();
				Audio.PlaySoundAtTransform("cSound", transform);
				input += "C";
				InputChecker();
		}
		public void OndButton()
		{
				GetComponent<KMSelectable>().AddInteractionPunch();
				Audio.PlaySoundAtTransform("dSound", transform);
				input += "D";
				InputChecker();
		}

}
