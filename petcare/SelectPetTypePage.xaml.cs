namespace petcare;

public partial class SelectPetTypePage : ContentPage
{
    private int currentStep = 1;

    public SelectPetTypePage()
    {
        InitializeComponent();
        ShowTypeStep();
    }

    private void ShowTypeStep()
    {
        currentStep = 1;

        StepTypeView.IsVisible = true;
        StepOtherTypeView.IsVisible = false;

        BottomButton.IsVisible = false;
    }

    private void ShowOtherTypeStep()
    {
        currentStep = 2;

        StepTypeView.IsVisible = false;
        StepOtherTypeView.IsVisible = true;

        BottomButton.IsVisible = true;
        BottomButton.Text = "Continue";

        EntryOtherPetType.Focus();
    }

    private async void OnDogSelected(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddPet("Dog"));
    }

    private async void OnCatSelected(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddPet("Cat"));
    }

    private void OnOtherSelected(object sender, EventArgs e)
    {
        ShowOtherTypeStep();
    }

    private async void OnBottomButtonClicked(object sender, EventArgs e)
    {
        string otherType = EntryOtherPetType.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(otherType))
        {
            await DisplayAlert("Error", "Please specify the type of pet.", "OK");
            return;
        }

        await Navigation.PushAsync(new AddPet(otherType));
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        if (currentStep == 1)
        {
            await Navigation.PopAsync();
        }
        else if (currentStep == 2)
        {
            ShowTypeStep();
        }
    }
}