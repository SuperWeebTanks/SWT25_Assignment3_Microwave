using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Unit.Integrationstest.IT4
{
    [TestFixture]
    public class Door_Button_Powertube
    {
        private ITimer _timer;
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
            _timer = Substitute.For<ITimer>();
            _light = Substitute.For<ILight>();
            _display = Substitute.For<IDisplay>();
            _output = Substitute.For<IOutput>();

            _startCancelButtonDriven = new Button();
            _powerButtonDriven = new Button();
            _timerButtonDriven = new Button();
            _doorDriven = new Door();
            _powerTubeToIntegrate = new PowerTube(_output);
            _cookController = new CookController(_timer, _display, _powerTubeToIntegrate);
            _userInterface = new UserInterface(_powerButtonDriven, _timerButtonDriven, _startCancelButtonDriven, _doorDriven, _display, _light, _cookController);
            _cookController.UI = _userInterface;
        }

        [Test]
        public void StartCooking_PowerTubeTurnsOn_OutputLineCalledWithCorrectArgument()
        {
            _powerButtonDriven.Press();
            _timerButtonDriven.Press();
            _startCancelButtonDriven.Press();

            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("PowerTube works")));
        }

        [Test]
        public void StopCooking_PowerTubeTurnsOff_OutputLineCalledWithCorrectArgument()
        {
            _powerButtonDriven.Press();
            _timerButtonDriven.Press();
            _startCancelButtonDriven.Press();
            _startCancelButtonDriven.Press();

            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("PowerTube turned off")));
        }
    }
}