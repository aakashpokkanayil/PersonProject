namespace ProjectTest.SampleUnitTests
{
    public class UnitTest1
    {
        [Fact] // Test method have to decorated with Attribute [Fact]
        public void Test1()
        {
            // there are 3 steps for each Unit Test.
            // 1.  Arrange : Declaration of Variables, collecting the inputs, creating objects etc.
            // 2. Act : Calling the method,which need to test.
            // 3.Assert: Here we compaire Expected value with actual value(from method).
            // if Expected value and actual value are same then test case passed else failed.

            // here we created a sample class MyMath, and in the class we add a Method Add
            // which adds 2 int inputs and returns the result.

            // Step1: Arrange
            MyMath myMath = new MyMath();
            int input1 = 10, input2 = 5;
            int expected = 15;

            // Step2: Act
            int actual = myMath.Add(input1, input2);

            // Step3: Assert
            Assert.Equal(expected, actual);

            // In order to test this , go to View >  Test Explorer then like solution Explorer a window opens.
            // we can use Test Explorer to test our project.
            // Click on double arrow in the top left.
        }
    }
}