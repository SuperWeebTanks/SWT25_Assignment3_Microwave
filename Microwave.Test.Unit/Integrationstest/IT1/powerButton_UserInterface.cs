namespace Microwave.Test.Unit.Integrationstest.IT1
{
    using MicrowaveOvenClasses.Boundary;
    using MicrowaveOvenClasses.Controllers;
    using MicrowaveOvenClasses.Interfaces;
    using NSubstitute;
    using NUnit.Framework;

        [TestFixture]
        public class powerButton_UserInterface
        {
            private IDoor _door;
            private IDisplay _display;
            private ILight _light;
            private IButton _startCancelButton;
            private IButton _powerButtonDriven;
            private IButton _timerButton;
            private ICookController _cooker;
            private IUserInterface _UIToIntegrate;

            [SetUp]
            public void SetUp()
            {
                _door = Substitute.For<IDoor>();
                _display = Substitute.For<IDisplay>();
                _light = Substitute.For<ILight>();
                _startCancelButton = Substitute.For<IButton>();
                _timerButton = Substitute.For<IButton>();
                _cooker = Substitute.For<ICookController>();

                _powerButtonDriven = new Button();
                _UIToIntegrate = new UserInterface(_powerButtonDriven, _timerButton, _startCancelButton,
                    _door, _display, _light, _cooker);
            }

            [TestCase(0)]
            [TestCase(1)]
            [TestCase(60)]
            public void UI_PowerButtonPressed_ShowPowerCalledCorrectNumberOfTimes(int number)
            {
                for (int i = 0; i < number; i++)
                    _powerButtonDriven.Press();

                _display.Received(number).ShowPower(Arg.Any<int>());
            }

            [Test]
            public void UI_PowerButtonPressedOnce_ShowPowerCalledWithCorrectArgument()
            {
                _powerButtonDriven.Press();

                _display.Received(1).ShowPower(50);
            }

            [Test]
            public void UI_PowerButtonPressedTwice_ShowPowerCalledWithCorrectArgument()
            {
                _powerButtonDriven.Press();
                _powerButtonDriven.Press();

                _display.Received().ShowPower(100);
            }

            [Test]
            public void UI_PowerButtonPressedFourteenTimes_ShowPowerCalledWithCorrectArgument()
            {
                for (int i = 0; i < 14; i++)
                    _powerButtonDriven.Press();

                _display.Received().ShowPower(650);
            }

            [Test]
            public void UI_PowerButtonPressedFifteenTimes_ShowPowerCalledWithCorrectArgument()
            {
                for (int i = 0; i < 15; i++)
                    _powerButtonDriven.Press();

                _display.Received(2).ShowPower(50);
            }

        [Test]
        public void UI_PowerButtonPressedInDoorOpenState_ShowPowerIsNotCalled()
        {
            _door.Opened += Raise.Event();

            _powerButtonDriven.Press();
            _display.DidNotReceive().ShowPower(Arg.Any<int>());
        }

        [Test]
        public void UI_PowerButtonPressedInCookingState_ShowPowerIsNotCalled()
        {
            _powerButtonDriven.Pressed += Raise.Event();
            _timerButton.Pressed += Raise.Event();
            _startCancelButton.Pressed += Raise.Event();

            _powerButtonDriven.Press();
            _display.DidNotReceive().ShowPower(100);
        }

        [Test]
        public void UI_PowerButtonPressedInSetTimeState_ShowPowerIsNotCalled()
        {
            _powerButtonDriven.Pressed += Raise.Event();
            _timerButton.Pressed += Raise.Event();

            _powerButtonDriven.Press();
            _display.DidNotReceive().ShowPower(100);
        }
    }
}