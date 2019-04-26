using System;
using System.Threading;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Unit.Integrationstest.IT4
{
    [TestFixture]
    public class Door_Button_Powertube
    {
        private ITimer _timerDriven;
        private IPowerTube _powerTubeToIntegrate;
        private ILight _light;
        private IDisplay _display;
        private IOutput _output;
        private CookController _cookController;
        private UserInterface _userInterface;
        private Door _doorDriven;
        private Button _startCancelButtonDriven;
        private Button _timerButtonDriven;
        private Button _powerButtonDriven;

        [SetUp]
        public void Setup()
        {
            _light = Substitute.For<ILight>();
            _display = Substitute.For<IDisplay>();
            _output = Substitute.For<IOutput>();

            _timerDriven = new Timer();
            _startCancelButtonDriven = new Button();
            _powerButtonDriven = new Button();
            _timerButtonDriven = new Button();
            _doorDriven = new Door();
            _powerTubeToIntegrate = new PowerTube(_output);
            _cookController = new CookController(_timerDriven, _display, _powerTubeToIntegrate);
            _userInterface = new UserInterface(_powerButtonDriven, _timerButtonDriven, _startCancelButtonDriven, _doorDriven, _display, _light, _cookController);
            _cookController.UI = _userInterface;
        }

        [Test]
        public void StartCooking_PowerTubeTurnsOn_OutputLineCalledCorrectly()
        {

            _powerButtonDriven.Press();
            _timerButtonDriven.Press();
            _startCancelButtonDriven.Press();

            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("PowerTube works")));
        }

        [Test]
        public void Stop_PowerTubeTurnsOff_OutputLineCalledCorrectly()
        {
            _doorDriven.Open();
            _doorDriven.Close();
            _powerButtonDriven.Press();
            _timerButtonDriven.Press();
            _startCancelButtonDriven.Press();
            _startCancelButtonDriven.Press();

            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("PowerTube turned off")));
        }

        [Test]
        public void OnTimerExpired_PowerTubeTurnsOff_OutputLineCalledCorrectly()
        {
            _doorDriven.Open();
            _doorDriven.Close();
            _powerButtonDriven.Press();
            _timerButtonDriven.Press();
            _startCancelButtonDriven.Press();
            Thread.Sleep(61000);

            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("PowerTube turned off")));
        }

        [Test]
        public void OnDoorOpened_WhileCookingPowerTubeTurnsOff_OutputLineCalledCorrectly()
        {
            _doorDriven.Open();
            _doorDriven.Close();
            _powerButtonDriven.Press();
            _timerButtonDriven.Press();
            _startCancelButtonDriven.Press();
            _doorDriven.Open();

            _output.Received().OutputLine(Arg.Is<string>(str => str.Equals("PowerTube turned off")));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(10)]
        [TestCase(13)]
        [TestCase(14)]
        [TestCase(15)]
        [TestCase(60)]
        public void StartCooking_PowerTubeTurnsOn_OutputLineCalledWithCorrectArguments(int number)
        {
            int power = 0;

            for (int i = 0; i < number; i++)
            {
                _powerButtonDriven.Press();
                power = (power >= 700 ? 50 : power + 50);
            }

            double powerPercentage = Math.Round((((double)power / 700) * 100), 2);

            _timerButtonDriven.Press();
            _startCancelButtonDriven.Press();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Equals($"PowerTube works with {powerPercentage} %")));
        }
    }
}