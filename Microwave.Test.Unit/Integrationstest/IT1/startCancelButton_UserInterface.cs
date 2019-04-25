using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Unit.Integrationstest.IT1
{
    [TestFixture]
    public class startCancelButton_UserInterface
    {
        //Stubs
        private IDoor _door;
        private IButton _powerButton;
        private IButton _timerButton;
        private IDisplay _display;
        private ILight _light;
        private ICookController _cookController;

        //Included Modules 
        private UserInterface _uiToIntegrate;

        //Driven Module 
        private Button _startCancelButtonDriven;

        [SetUp]
        public void SetUp()
        {
            //Stubs
            _door = Substitute.For<IDoor>();
            _powerButton = Substitute.For<IButton>();
            _timerButton = Substitute.For<IButton>();
            _display = Substitute.For<IDisplay>();
            _light = Substitute.For<ILight>();
            _cookController = Substitute.For<ICookController>();

            //DrivenModule
            _startCancelButtonDriven = new Button();

            //Module to integrate
            _uiToIntegrate = new UserInterface(_powerButton, _timerButton,
                _startCancelButtonDriven, _door, _display, _light, _cookController);

        }

        //User presses start cancel button during cooking (Extension 3)
        [Test]
        public void UI_UserPressesStartCancelButtonDuringCooking_StopCooking()
        {
            //Act
            _powerButton.Pressed += Raise.Event();
            _timerButton.Pressed += Raise.Event();
            _startCancelButtonDriven.Press();
            _startCancelButtonDriven.Press();

            //Assert
            _cookController.Received(1).Stop();

        }

        //User presses start cancel button during cooking (Extension 3)
        [Test]
        public void UI_UserPressesStartCancelButtonDuringCooking_DisplayBlanked()
        {
            //Act
            _powerButton.Pressed += Raise.Event();
            _timerButton.Pressed += Raise.Event();
            _startCancelButtonDriven.Press();
            _startCancelButtonDriven.Press();

            //Assert
            _display.Received(2).Clear();

        }

        //User presses start cancel button during cooking (Extension 3)
        [Test]
        public void UI_UserPressesStartCancelButtonDuringCooking_LightOff()
        {
            //Act
            _powerButton.Pressed += Raise.Event();
            _timerButton.Pressed += Raise.Event();
            _startCancelButtonDriven.Press();
            _startCancelButtonDriven.Press();

            //Assert 
            _light.Received(1).TurnOff();
        }

        //User presses start cancel button during cooking (Extension 3)
        [Test]
        public void UI_UserPressesStartCancelTwiceButtonDuringCooking_NothingHappensOnSecondPress()
        {
            //Act
            _powerButton.Pressed += Raise.Event();
            _timerButton.Pressed += Raise.Event();
            _startCancelButtonDriven.Press();
            _startCancelButtonDriven.Press();
            //Second Press
            _startCancelButtonDriven.Press();

            //Assert
            _cookController.Received(1).Stop();
            _display.Received(2).Clear();
            _light.Received(1).TurnOff();

        }

        
        //User presses start cancel button under power setup (Extension 1) 
        [Test]
        public void UI_UserPressesStartCancelButtonUnderPowerSetup_DisplayCleared()
        {
            //Act 
            _powerButton.Pressed += Raise.Event();
            _startCancelButtonDriven.Press();

            //Assert
            _display.Received(1).Clear();
        }

        //User presses start cancel button twice at start 
        [Test]
        public void UI_UserPressesStartCancelButtonTwice_NothingHappensOnSecondPress()
        {
            //Act 
            _powerButton.Pressed += Raise.Event();
            _startCancelButtonDriven.Press();
            _startCancelButtonDriven.Press();

            //Assert
            _display.Received(1).ShowPower(50);
            _display.Received(1).Clear();
        }

        [Test]
        public void UI_UserPressesStartCancelButtonAfterTimerSetup_LightOn()
        {
            //Act
            _powerButton.Pressed += Raise.Event();
            _timerButton.Pressed += Raise.Event();
            _startCancelButtonDriven.Press();
            
            //Assert
            _light.Received().TurnOn(); 

        }

        [Test]
        public void UI_UserPressesStartCancelButtonAfterTimerSetup_StartCooking()
        {
            //Act
            _powerButton.Pressed += Raise.Event();
            _timerButton.Pressed += Raise.Event();
            _startCancelButtonDriven.Press();

            //Assert
            _cookController.StartCooking(50,60);
           
        }
    }
}
