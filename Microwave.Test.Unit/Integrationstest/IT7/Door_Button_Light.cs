using System.Threading;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Unit.Integrationstest.IT7
{
    [TestFixture]
    public class Door_Button_Light
    {
        //Stubbed Modules
        private IOutput _output;

        //To Integrate Module
        private Light _lightToIntegrate;
        
        //Included Modules
        private Timer _timer;
        private PowerTube _powerTube;
        private CookController _cookController;
        private UserInterface _userInterface;
        private Display _display;


        //Driven Modules
        private Door _doorDriven;
        private Button _startCancelButtonDriven;
        private Button _timerButtonDriven;
        private Button _powerButtonDriven;

        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            
            _timer = new Timer();
            _powerTube = new PowerTube(_output);
            _display = new Display(_output);
            _doorDriven = new Door();
            _startCancelButtonDriven = new Button();
            _powerButtonDriven = new Button();
            _timerButtonDriven = new Button();
            _lightToIntegrate = new Light(_output);

            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_powerButtonDriven, _timerButtonDriven, _startCancelButtonDriven, _doorDriven, _display, _lightToIntegrate, _cookController);
            _cookController.UI = _userInterface;
        }

        [Test]
        public void OutPutLine_UserOpensDoor_LightOn()
        {
            //Act
            _doorDriven.Open();

            //Assert
            _output.Received(1).OutputLine(Arg.Is("Light is turned on"));
        }

        [Test]
        public void OutPutLine_UserClosesDoor_LightOff()
        {
            //Act
            _doorDriven.Open();
            _doorDriven.Close();

            //Assert
            _output.Received(1).OutputLine(Arg.Is("Light is turned off"));
        }

        [Test]
        public void OutPutLine_UserPressesOnStartCancelButtonAfterSetup_TurnOn()
        {
            //Act
            _doorDriven.Open();
            _doorDriven.Close();
            _powerButtonDriven.Press();
            _timerButtonDriven.Press();
            _startCancelButtonDriven.Press();

            //Assert
            _output.Received(2).OutputLine(Arg.Is("Light is turned on"));

        }

        [Test]
        public void OutPutLine_CookingIsDone_TurnOff()
        {
            //Act
            _doorDriven.Open();
            _doorDriven.Close();
            _powerButtonDriven.Press();
            _timerButtonDriven.Press();
            _startCancelButtonDriven.Press();

            Thread.Sleep(61*1000);

            //Assert
            _output.Received(2).OutputLine(Arg.Is("Light is turned off"));
        }

        [Test]
        public void OutPutLine_UserPressesStartCancelButtonDuringCooking_TurnOff()
        {
            //Act
            _doorDriven.Open();
            _doorDriven.Close();
            _powerButtonDriven.Press();
            _timerButtonDriven.Press();
            _startCancelButtonDriven.Press();
            _startCancelButtonDriven.Press();

            //Assert
            _output.Received(2).OutputLine(Arg.Is("Light is turned off"));
        }

    }
}
