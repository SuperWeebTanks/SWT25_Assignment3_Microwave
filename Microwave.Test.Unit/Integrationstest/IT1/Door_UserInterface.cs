using System;
using System.ComponentModel.Design;
using Castle.Core.Smtp;
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
        public void TurnOn_UserOpensDoor_TurnOnIsCalled()
        {
            //Act
            _doorDriven.Open();

            //Assert
            _light.Received(1).TurnOn();
        }

        [Test]
        //Open already Opened door 
        public void TurnOn_UserOpensAlreadyOpenedDoor_TurnOnIsNotCalled()
        {
            //Act
            _doorDriven.Open();
            _doorDriven.Open();

            _light.Received(1).TurnOn();
        }

        [Test]
        //Close door, light received TurnOff() (Normal SD) 
        public void TurnOff_UserClosesOpenedDoor_TurnOffIsCalled()
        {
            //Act
            _doorDriven.Open();
            _doorDriven.Close();

            //Assert
            _light.Received(1).TurnOff();
        }


        [Test]
        public void TurnOff_UserClosesDoorButDoorIsAlreadyClosed_TurnOffIsNotCalled()
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
        public void TurnOn_UserOpensDoorDuringSetup_LightOn()
        {
            //Act 
            _powerButton.Pressed += Raise.Event(); 
            _doorDriven.Open();

            //Assert
            _light.Received(1).TurnOn();

        }

        [Test]
        public void Clear_UserOpensDoorDuringSetup_DisplayCleared()
        {
            //Act 
            _powerButton.Pressed += Raise.Event();
            _doorDriven.Open();

            //Assert
            _display.Received(1).Clear();
        }

        //Open door, CookController received Stop() (Extension 4)
        //User opens the door during cooking 
        [Test]
        public void Stop_UserOpensDoorDuringCooking_CookingStopsDisplayBlanked()
        {
            //Act
            _powerButton.Pressed += Raise.Event();
            _timerButton.Pressed += Raise.Event();
            _startCancelButton.Pressed += Raise.Event();
            _doorDriven.Open();

            //Assert 
            _cookController.Received(1).Stop();
            
        }

        [Test]
        public void Clear_UserOpensDoorDuringCooking_DisplayCleared()
        {
            //Act
            _powerButton.Pressed += Raise.Event();
            _timerButton.Pressed += Raise.Event();
            _startCancelButton.Pressed += Raise.Event();
            _doorDriven.Open();

            //Assert 
            _display.Received(1).Clear();

        }

        //Step 15, User opens door efter timer has expired and food is ready 
        [Test]
        public void TurnOn_UserOpensDoorAfterFoodIsReady_LightOn()
        {
            //Act
            _powerButton.Pressed += Raise.Event();
            _timerButton.Pressed += Raise.Event();
            _startCancelButton.Pressed += Raise.Event();
           _uiToIntegrate.CookingIsDone();
           _doorDriven.Open();

            //Assert
            _light.Received(2).TurnOn();
        }

        //Step 18, User closes door after removing food. 
        [Test]
        public void TurnOff_UserClosesDoorAfterRemovingFood_LightOff()
        {
            //Act
            _powerButton.Pressed += Raise.Event();
            _timerButton.Pressed += Raise.Event();
            _startCancelButton.Pressed += Raise.Event();
            _uiToIntegrate.CookingIsDone();
            _doorDriven.Open();
            _doorDriven.Close();

            //Assert
            _light.Received(2).TurnOff();
        }

    }
}
