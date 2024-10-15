global using System;
global using System.Collections;
global using System.Collections.Concurrent;
global using System.Collections.Generic;
global using System.ComponentModel;
global using System.Diagnostics;
global using System.Linq;
global using System.Reflection;
global using System.Runtime.CompilerServices;
global using System.Threading.Tasks;

global using System.Windows.Input;

global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;

global using Avalonia;
global using Avalonia.Controls;
global using Avalonia.Controls.ApplicationLifetimes;
global using Avalonia.Data.Core.Plugins;
global using Avalonia.Markup.Xaml;
global using Avalonia.Threading;
global using Avalonia.Interactivity;
global using Avalonia.Layout;
global using Avalonia.Media;

global using SmartGenealogy.Avalonia.Interfaces;
global using SmartGenealogy.Avalonia.Interfaces.Messenger;
global using SmartGenealogy.Avalonia.Interfaces.Logger;

global using SmartGenealogy.Avalonia.Mvvm;
global using SmartGenealogy.Avalonia.Mvvm.Core;
global using SmartGenealogy.Avalonia.Mvvm.Messenger;
global using SmartGenealogy.Avalonia.Mvvm.Utilities;
global using SmartGenealogy.StateMachine;