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
        private CookController _cookControllerToIntegrate;
        private UserInterface _userInterface;
        private Door _doorDriven;
        private Button _startCancelButtonDriven;
        private Button _timerButtonDriven;
        private Button _powerButtonDriven;

        [SetUp]
        public void Setup()
        {
            _startCancelButtonDriven = new Button();
            _powerButtonDriven = new Button();
            _timerButtonDriven = new Button();
            _doorDriven=new Door();
            _timer = Substitute.For<ITimer>();
            _powerTube = Substitute.For<IPowerTube>();
            _light= Substitute.For<ILight>();
            _display= Substitute.For<IDisplay>();
            _cookControllerToIntegrate= new CookController(_timer,_display,_powerTube);
            _userInterface=new UserInterface(_powerButtonDriven,_timerButtonDriven,_startCancelButtonDriven,_doorDriven,_display,_light,_cookControllerToIntegrate);
            _cookControllerToIntegrate.UI = _userInterface;
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
            _doorDriven.Open();
            _doorDriven.Close();
            for (int i = 0; i < x; i++)
            {
                _powerButtonDriven.Press();
            }
            _timerButtonDriven.Press();
            _startCancelButtonDriven.Press();
            _powerTube.Received().TurnOn(Arg.Is(x*50));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void StartCooking_TimerReceivesCorrectTime(int x)
        {
            _doorDriven.Open();
            _doorDriven.Close();
            _powerButtonDriven.Press();
            for (int i = 0; i < x; i++)
            {
                _timerButtonDriven.Press();
            }
            _startCancelButtonDriven.Press();
            _timer.Received().Start(Arg.Is(x*60));

        }
    }
}
