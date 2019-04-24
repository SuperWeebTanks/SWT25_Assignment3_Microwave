using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            //Module Driver
            _startCancelButtonDriven = new Button();

            //Module to integrate
            _uiToIntegrate = new UserInterface(_powerButton, _timerButton,
                _startCancelButtonDriven, _door, _display, _light, _cookController);

        }

    }
}
