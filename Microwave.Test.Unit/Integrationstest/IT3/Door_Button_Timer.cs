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
        private ITimer _timerToIntegrate;
        private IPowerTube _powerTube;
        private ILight _light;
        private IDisplay _display;
        private CookController _cookController;
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
            _doorDriven = new Door();
            _timerToIntegrate = new Timer();
            _powerTube = Substitute.For<IPowerTube>();
            _light = Substitute.For<ILight>();
            _display = Substitute.For<IDisplay>();
            _cookController = new CookController(_timerToIntegrate, _display, _powerTube);
            _userInterface = new UserInterface(_powerButtonDriven, _timerButtonDriven, _startCancelButtonDriven, _doorDriven, _display, _light, _cookController);
            _cookController.UI = _userInterface;
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Start_TimeRemaining(int x)
        {
            _doorDriven.Open();
            _doorDriven.Close();
            _powerButtonDriven.Press();
            for (int i = 0; i < x; i++)
            {
                _timerButtonDriven.Press();
            }
            _startCancelButtonDriven.Press();
            Assert.That(_timerToIntegrate.TimeRemaining,Is.EqualTo(x*60));
        }

        [Test]
        public void OnTimerEvent_TimerTickCalled_SixtyTimes()
        {
            int ticks = 0;
            _timerToIntegrate.TimerTick+= (sender, args) => ticks++;
            _doorDriven.Open();
            _doorDriven.Close();
            _powerButtonDriven.Press();
            _timerButtonDriven.Press();
            _startCancelButtonDriven.Press();
            Thread.Sleep(61000);
            Assert.That(ticks,Is.EqualTo(60));
        }

        [Test]
        public void Expire_ExpireCalled_OneTime()
        {
            int tick = 0;
            _timerToIntegrate.Expired += (sender, args) => tick++;
            _doorDriven.Open();
            _doorDriven.Close();
            _powerButtonDriven.Press();
            _timerButtonDriven.Press();
            _startCancelButtonDriven.Press();
            Thread.Sleep(61000);
            Assert.That(tick, Is.EqualTo(1));
        }
    }
}
