using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Unit.Integrationstest.IT1
{
    [TestFixture]
    public class timerButton_UserInterface
    {
        private IDoor _door;
        private IDisplay _display;
        private ILight _light;
        private IButton _startCancelButton;
        private IButton _powerButton;
        private IButton _timerButtonDriven;
        private ICookController _cooker;
        private IUserInterface _UIToIntegrate;

        [SetUp]
        public void SetUp()
        {
            _door = Substitute.For<IDoor>();
            _display = Substitute.For<IDisplay>();
            _light = Substitute.For<ILight>();
            _startCancelButton = Substitute.For<IButton>();
            _powerButton = Substitute.For<IButton>();
            _cooker = Substitute.For<ICookController>();

            _timerButtonDriven = new Button();
            _UIToIntegrate = new UserInterface(_powerButton, _timerButtonDriven, _startCancelButton,
                _door, _display, _light, _cooker);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(60)]
        public void UI_TimerButtonPressedRepeatedly_ShowTimeCalledCorrectNumberOfTimes(int number)
        {
            _powerButton.Pressed += Raise.Event();

            for (int i = 0; i < number; i++)
                _timerButtonDriven.Press();

            _display.Received(number).ShowTime(Arg.Any<int>(), Arg.Any<int>());
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(60)]
        public void UI_TimerButtonPressedRepeatedly_ShowTimeCalledWithCorrectArguments(int number)
        {
            _powerButton.Pressed += Raise.Event();

            for (int i = 0; i < number; i++)
                _timerButtonDriven.Press();

            _display.Received().ShowTime(number, 0);
        }

        [Test]
        public void UI_TimerButtonPressedInReadyState_ShowTimeIsNotCalled()
        {
            _timerButtonDriven.Press();
            _display.DidNotReceive().ShowTime(Arg.Any<int>(), Arg.Any<int>());
        }

        [Test]
        public void UI_TimerButtonPressedInCookingState_ShowTimeIsNotCalled()
        {
            _powerButton.Pressed += Raise.Event();
            _timerButtonDriven.Pressed += Raise.Event();
            _startCancelButton.Pressed += Raise.Event();

            _timerButtonDriven.Press();
            _display.DidNotReceive().ShowTime(Arg.Any<int>(), Arg.Any<int>());
        }

        [Test]
        public void UI_TimerButtonPressedInDoorOpenState_ShowTimeIsNotCalled()
        {
            _door.Open();
            _timerButtonDriven.Press();
            _display.DidNotReceive().ShowTime(Arg.Any<int>(), Arg.Any<int>());
        }
    }
}