using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private IUserInterface _ui;

        //Included Modules 
        private Button _startCancelButton;
        private Button _powerButton;
        private Button _timerButton;
        private Door _door;


        //Driven Module 
        private CookController _cookControllerDriven;

        [SetUp]
        public void SetUp()
        {
            _startCancelButton = new Button();
            _powerButton = new Button();
            _timerButton = new Button();
            _door = new Door();
            _cookControllerDriven = new CookController(_timer, _display, _tube, _ui);
        }



    }
}
