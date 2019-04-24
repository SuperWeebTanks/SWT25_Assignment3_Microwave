using System;
using System.ComponentModel.Design;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NUnit.Framework;
using NSubstitute;

namespace Microwave.Test.Unit.Integrationstest.IT1
{
    [TestFixture]
    public class Door_UserInterface
    {
        //Stubs
        private IButton _startCancelButton;
        private IButton _powerButton;
        private IButton _timerButton;
        private IDisplay _display;
        private ILight _light;
        private ICookController _cookController; 

        //Included Modules 
        private UserInterface _uiToIntegrate;

        //Driven Module 
        private Door _doorDriven;
        
        [SetUp]
        public void SetUp()
        {
            //Stubs
            _startCancelButton = Substitute.For<IButton>();
            _powerButton = Substitute.For<IButton>();
            _timerButton = Substitute.For<IButton>();
            _display = Substitute.For<IDisplay>();
            _light = Substitute.For<ILight>();
            _cookController = Substitute.For<ICookController>();

            //Module Driver
            _doorDriven = new Door();

            //Module to integrate
            _uiToIntegrate = new UserInterface(_powerButton, _timerButton, 
                _startCancelButton, _doorDriven, _display, _light, _cookController);
            
        }

        //Open door, light received TurnON() (Normal SD)
        [Test]
        public void UI_UserOpensDoor_TurnOnIsCalled()
        {
            //Act
            _doorDriven.Open();

            //Assert
            _light.Received().TurnOn();
        }

        [Test]
        //Open already Opened door 
        public void UI_UserOpensAlreadyOpenedDoor_TurnOnIsNotCalled()
        {
            //Act
            _doorDriven.Open();
            _doorDriven.Open();

            _light.Received(1).TurnOn();
        }

        [Test]
        //Close door, light received TurnOff() (Normal SD) 
        public void UI_UserClosesOpenedDoor_TurnOffIsCalled()
        {
            //Act
            _doorDriven.Open();
            _doorDriven.Close();

            //Assert
            _light.Received().TurnOff();
        }


        [Test]
        public void UI_UserClosesDoorButDoorIsAlreadyClosed_TurnOffIsNotCalled()
        {
            //Act
            _doorDriven.Open();
            _doorDriven.Close();
            _doorDriven.Close();

            //Assert
            _light.Received(1).TurnOff();
        }


        //Open door, CookController received Stop() (Extension 2) 
        //User opens the door during setup 
        [Test]
        public void UI_UserOpensDoorDuringSetup_LightOnDisplayBlanked()
        {
            //Act 
            _powerButton.Pressed += Raise.EventWith(new object(), new EventArgs()); 
            _doorDriven.Open();

            //Assert
            _light.Received().TurnOn();
            _display.Received().Clear();

        }

        //Open door, CookController received Stop() (Extension 4)
        //User opens the door during cooking 
        [Test]
        public void UI_UserOpensDoorDuringCooking_PowerTubeOffDisplayBlanked()
        {
            //Act
            _powerButton.Press();
            _timerButton.Press();
            _startCancelButton.Press();
            _doorDriven.Open();

            //Assert 
            

        }



    }
}
