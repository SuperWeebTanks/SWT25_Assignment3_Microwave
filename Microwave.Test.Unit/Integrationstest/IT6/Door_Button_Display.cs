using System;
using System.Security.Policy;
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
        public void OutPutLine_ShowPowerOutPut_LogsLineCorrectPowerLevel(
            [Values(1,2,3,4,5,6,7,8,9,9,10,11,100)] int x)
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
        public void OutPutLine_ShowTimerOutPut_LogsCorrectLine(
            [Values(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 99, 100, 101)] int x)
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

        [Test]
        public void OutPutLine_ShowTimeTicks_LogsLineForMinutes(
            [Values(1,2)] int x)
        {
            //Act 
            _powerButtonDriven.Press();
            for (int i = 0; i < x; ++i)
            {
                _timerButtonDriven.Press();
            }
            _startCancelButtonDriven.Press();

            Thread.Sleep(x*61*1000);

            _output.Received(1).OutputLine(Arg.Is<string>($"Display shows: {x:D2}:00"));
            _output.Received(1).OutputLine(Arg.Is<string>($"Display shows: 00:00"));
            for (int i = 1; i<=x-1; i++)
            {
                _output.Received(2).OutputLine(Arg.Is<string>($"Display shows: {i:D2}:00"));
            }
        }

        
        [Test]
        public void OutPutLine_ShowTimeTicks_LogsLineForSeconds(
            [Values(1,2)] int x)
        {
            //Act 
            _powerButtonDriven.Press();
            for (int i = 0; i < x; ++i)
            {
                _timerButtonDriven.Press();
            }
            _startCancelButtonDriven.Press();

            Thread.Sleep(x * 61 * 1000);

            for (int j = 0; j <= x-1; j++)
            {
                for (int i = 1; i <= 59; i++)
                {
                    _output.Received(i == 0 || (j == 0  && i == 0) ? 2 : 1).OutputLine(Arg.Is<string>($"Display shows: {j:D2}:{i:D2}"));
                }
            }
        }

        [Test]
        public void OutPutLine_CookingIsDone_OutputCleared()
        {
            //Act
            _powerButtonDriven.Press();
            _timerButtonDriven.Press();
            _startCancelButtonDriven.Press();
            Thread.Sleep(61*1000);

            //Assert
            _output.Received(2).OutputLine(Arg.Is("Display cleared"));
        }

        [Test]
        public void OutPutLine_UserOpensDoorDuringSetup_DisplayCleared()
        {
            //Act
            _powerButtonDriven.Press();
            _doorDriven.Open();

            //Assert
            _output.Received(1).OutputLine(Arg.Is("Display cleared"));
        }

        [Test]
        public void OutPutLine_UserOpensDoorDuringCooking_DisplayCleared()
        {
            //Act
            _powerButtonDriven.Press();
            _timerButtonDriven.Press();
            _startCancelButtonDriven.Press();
            _doorDriven.Open();

            //Assert
            _output.Received(1).OutputLine(Arg.Is("Display cleared"));

        }

        [Test]
        public void OutPutLine_UserPressesStartCancelButton_DisplayCleared()
        {
            //Act
            _powerButtonDriven.Press();
            _startCancelButtonDriven.Press();

            //Assert
            //Assert
            _output.Received(1).OutputLine(Arg.Is("Display cleared"));
        }

        [Test]
        public void OutPutLine_UserPressesStartCancelButtonDuringCooking_DisplayCleared()
        {
            //Act
            _powerButtonDriven.Press();
            _timerButtonDriven.Press();
            _startCancelButtonDriven.Press();
            _startCancelButtonDriven.Press();

            //Assert
            _output.Received().OutputLine(Arg.Is("Display cleared"));
        }


    }
}
