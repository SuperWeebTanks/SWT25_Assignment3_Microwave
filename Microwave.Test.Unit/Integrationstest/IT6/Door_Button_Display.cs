﻿using System;
using System.Threading;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Unit.Integrationstest.IT5
{
    [TestFixture]
    public class Door_Button_Display
    {
        //Stubbed Modules
        private ILight _light;
        private IOutput _output; 

        //To Integrate Module
        private Display _displayToIntegrate;

        //Included Modules
        private Timer _timer;
        private PowerTube _powerTube;
        private CookController _cookController;
        private UserInterface _userInterface;

        //Driven Modules
        private Door _doorDriven;
        private Button _startCancelButtonDriven;
        private Button _timerButtonDriven;
        private Button _powerButtonDriven;

        [SetUp]
        public void SetUp()
        {
            _light = Substitute.For<ILight>();
            _output = Substitute.For<IOutput>(); 

            _timer = new Timer();
            _powerTube = new PowerTube(_output);
            _displayToIntegrate = new Display(_output);
            _doorDriven = new Door();
            _startCancelButtonDriven = new Button();
            _powerButtonDriven = new Button();
            _timerButtonDriven = new Button();

            _cookController = new CookController(_timer, _displayToIntegrate, _powerTube);
            _userInterface = new UserInterface(_powerButtonDriven, _timerButtonDriven, _startCancelButtonDriven, _doorDriven, _displayToIntegrate, _light, _cookController);
            _cookController.UI = _userInterface; 
        }


        [Test]
        public void Display_ShowPowerOutPut_LogsLineCorrectPowerLevel(
            [Values(1,2,3,4,5,6,7,8,9,9,10,11,12,13,14,15,16,100)] int x)
        {
            //Act
            for (int i = 0; i <= x; i++)
            {
                _powerButtonDriven.Press();
            }

            //Assert
            _output.Received()
                .OutputLine(x >= 15 ? Arg.Is<string>($"Display shows: {700} W") : Arg.Is<string>($"Display shows: {x * 50} W"));
        }

        [Test]
        public void Display_ShowTimerOutPut_LogsCorrectLine(
            [Values(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 98, 99, 100, 101, 102)] int x)
        {
            //Act
            _powerButtonDriven.Press();
            for (int i = 0; i <= x; i++)
            {
                _timerButtonDriven.Press();
            }
            _timerButtonDriven.Press();

            //Assert
            //No requirement that it should not show more than 2 digits for minutes
            _output.Received(1)
                .OutputLine(x >= 10 ? Arg.Is<string>($"Display shows: {x}:00") : Arg.Is<string>($"Display shows: 0{x}:00"));
        }

        [Ignore("Not Ready")]
        [Test]
        public void Display_ShowTimeTicksAtDifferentPowerLevels_LogsLine(
            [Values(1, 2, 3, 4, 5, 6, 7, 8, 9, 10)] int x, [Values(1, 2, 3, 4, 5, 6, 7, 8, 9, 9, 10, 11, 12, 13, 14)] int y)
        {
            //Act
            for (int i = 0; i <= y; i++)
            {
                _powerButtonDriven.Press();
            }
            
            for (int i = 0; i <= x; i++)
            {
                _timerButtonDriven.Press();
            }
            Thread.Sleep(x*61*1000);

            for (int i = 0; i <= x * 60; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    _output.Received(1)
                        .OutputLine(x >= 10 ? Arg.Is<string>($"Display shows: {x}:" + (j >= 10 ? $"j" : $"0{j}")) : Arg.Is<string>($"Display shows: 0{x}:" +
                                                                                                        (j >= 10 ? $"j" : $"0{j}")));
                }
            }
        }

        [Test]
        public void Display_CookingIsDone_OutputCleared()
        {
            //Act
            _powerButtonDriven.Press();
            _timerButtonDriven.Press();
            _startCancelButtonDriven.Press();
            Thread.Sleep(61*1000);

            //Assert
            _output.Received(2).OutputLine(Arg.Is("Display cleared"));
        }

    }
}
