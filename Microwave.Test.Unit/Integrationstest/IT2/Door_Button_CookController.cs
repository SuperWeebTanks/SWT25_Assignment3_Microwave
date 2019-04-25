using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;

namespace Microwave.Test.Unit.Integrationstest.IT2
{
    [TestFixture]
    class IT2_CookController
    {
        private ITimer _timer;
        private IPowerTube _powerTube;
        private ILight _light;
        private IDisplay _display;
        private CookController _cookController;
        private UserInterface _userInterface;
        private Door _door;
        private Button _startCancelButton;
        private Button _timerButton;
        private Button _powerButton;

        [SetUp]
        public void Setup()
        {
            _startCancelButton = new Button();
            _powerButton = new Button();
            _timerButton = new Button();
            _door=new Door();
            _timer = Substitute.For<ITimer>();
            _powerTube = Substitute.For<IPowerTube>();
            _light= Substitute.For<ILight>();
            _display= Substitute.For<IDisplay>();
            _cookController= new CookController(_timer,_display,_powerTube);
            _userInterface=new UserInterface(_powerButton,_timerButton,_startCancelButton,_door,_display,_light,_cookController);
            _cookController.UI = _userInterface;
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(12)]
        [TestCase(13)]
        [TestCase(14)]
        public void StartCooking_PowerTubeReceivesCorrectPowerArgument(int x)
        {
            _door.Open();
            _door.Close();
            for (int i = 0; i < x; i++)
            {
                _powerButton.Press();
            }
            _timerButton.Press();
            _startCancelButton.Press();
            _powerTube.Received().TurnOn(Arg.Is(x*50));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void StartCooking_TimerReceivesCorrectTime(int x)
        {
            _door.Open();
            _door.Close();
            _powerButton.Press();
            for (int i = 0; i < x; i++)
            {
                _timerButton.Press();
            }
            _startCancelButton.Press();
            _timer.Received().Start(Arg.Is(x*60));

        }

       

    }
}
