using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core.Smtp;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Unit.Integrationstest.IT3
{
    [TestFixture]
    class Door_Button_Timer
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
            _door = new Door();
            _timer = new Timer();
            _powerTube = Substitute.For<IPowerTube>();
            _light = Substitute.For<ILight>();
            _display = Substitute.For<IDisplay>();
            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_powerButton, _timerButton, _startCancelButton, _door, _display, _light, _cookController);
            _cookController.UI = _userInterface;
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Start_TimeRemaining(int x)
        {
            _door.Open();
            _door.Close();
            _powerButton.Press();
            for (int i = 0; i < x; i++)
            {
                _timerButton.Press();
            }
            _startCancelButton.Press();
            Assert.That(_timer.TimeRemaining,Is.EqualTo(x*60));
        }

        [Test]
        public void OnTimerEvent_TimerTickCalled_SixtyTimes()
        {
            int ticks = 0;
            _timer.TimerTick+= (sender, args) => ticks++;
            _door.Open();
            _door.Close();
            _powerButton.Press();
            _timerButton.Press();
            _startCancelButton.Press();
            Thread.Sleep(61000);
            Assert.That(ticks,Is.EqualTo(60));
        }

        [Test]
        public void Expire_ExpireCalled_OneTime()
        {
            int tick = 0;
            _timer.Expired += (sender, args) => tick++;
            _door.Open();
            _door.Close();
            _powerButton.Press();
            _timerButton.Press();
            _startCancelButton.Press();
            Thread.Sleep(61000);
            Assert.That(tick, Is.EqualTo(1));
        }
    }
}
