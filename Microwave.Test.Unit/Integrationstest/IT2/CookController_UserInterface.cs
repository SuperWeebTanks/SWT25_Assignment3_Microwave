using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Castle.DynamicProxy.Contributors;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NUnit.Framework;
using NSubstitute;

namespace Microwave.Test.Unit.Integrationstest.IT3
{
    [TestFixture]
    class CookController_UserInterface
    {
        //Stubs
        private IDisplay _display;
        private ILight _light;
        private ITimer _timer;
        private IPowerTube _tube;
        
        //Included Modules 
        private Button _startCancelButton;
        private Button _powerButton;
        private Button _timerButton;
        private Door _door;
        private UserInterface _uiToIntegrate;


        //Driven Module 
        private CookController _cookControllerDriven;

        [SetUp]
        public void SetUp()
        {
            _display = Substitute.For<IDisplay>();
            _light = Substitute.For<ILight>();
            _timer = Substitute.For<ITimer>();
            _tube = Substitute.For<IPowerTube>();

            _startCancelButton = new Button();
            _powerButton = new Button();
            _timerButton = new Button();
            _door = new Door();
            _cookControllerDriven = new CookController(_timer, _display, _tube);
            _uiToIntegrate = new UserInterface(_powerButton, _timerButton, _startCancelButton, _door, _display, _light, _cookControllerDriven);
            _cookControllerDriven.UI = _uiToIntegrate;
        }

        [Test]
        public void CookController_TimerExpiresAndCookControllerInformsUI_DisplayCleared()
        {
            //Act 
            _powerButton.Press();
            _timerButton.Press();
            _startCancelButton.Press();

           _timer.Expired += Raise.Event();

            //Assert
            _display.Received().Clear();
        }

        [Test]
        public void CookController_TimerExpiresAndCookControllerInformsUI_TurnOffLight()
        {
            //Act 
            _powerButton.Press();
            _timerButton.Press();
            _startCancelButton.Press();

            _timer.Expired += Raise.Event();

            //Assert
            _light.Received().TurnOff();
        }

    }
}
