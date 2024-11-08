using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SimpleToDoList.Models;

namespace SimpleToDoList.ViewModels;

/// <summary>
/// This is our MainViewModel in which we define the ViewModel logic to interact between our View and the TodoItems
/// </summary>
public partial class MainViewModel : ViewModelBase
{
    public MainViewModel(IConfiguration configuration, ILogger<MainViewModel> logger)
    {
        // We can use this to add some items for the designer. 
        // You can also use a DesignTime-ViewModel
        _logger = logger;
        _configuration = configuration;

        if (Design.IsDesignMode)
        {
            ToDoItems = new ObservableCollection<ToDoItemViewModel>(new[]
            {
                new ToDoItemViewModel() { Content = "Hello" },
                new ToDoItemViewModel() { Content = "Avalonia", IsChecked = true}
            });
        }

        TitleText = configuration["Todo:Title"] ?? "Hello Title";

        _logger.LogInformation("MainViewModel created");
    }

    public string TitleText { get; init; }

    private readonly ILogger<MainViewModel> _logger;
    private readonly IConfiguration _configuration;


    /// <summary>
    /// Gets a collection of <see cref="ToDoItem"/> which allows adding and removing items
    /// </summary>
    public ObservableCollection<ToDoItemViewModel> ToDoItems { get; } = new ObservableCollection<ToDoItemViewModel>();


    // -- Adding new Items --

    /// <summary>
    /// This command is used to add a new Item to the List
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanAddItem))]
    private void AddItem()
    {
        // Add a new item to the list
        var item = new ToDoItemViewModel() { Content = NewItemContent };
        ToDoItems.Add(item);
        _logger.LogInformation("Added {Item}", item.Content);
        // reset the NewItemContent
        NewItemContent = null;
    }

    /// <summary>
    /// Gets or set the content for new Items to add. If this string is not empty, the AddItemCommand will be enabled automatically
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddItemCommand))] // This attribute will invalidate the command each time this property changes
    private string? _newItemContent;

    /// <summary>
    /// Returns if a new Item can be added. We require to have the NewItem some Text
    /// </summary>
    private bool CanAddItem() => !string.IsNullOrWhiteSpace(NewItemContent);

    // -- Removing Items --

    /// <summary>
    /// Removes the given Item from the list
    /// </summary>
    /// <param name="item">the item to remove</param>
    [RelayCommand]
    private void RemoveItem(ToDoItemViewModel item)
    {
        // Remove the given item from the list
        ToDoItems.Remove(item);
        _logger.LogInformation("Deleted {Item}", item.Content);
    }

    [RelayCommand]
    private void LogData()
    {
        _logger.LogDebug("The list has {Count} items", ToDoItems.Count);
    }

    [RelayCommand]
    private void ChangeLogLevel()
    {
        _logger.LogInformation("Not Changing LogLevel");
    }
}