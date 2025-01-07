# nopCommerce Basic Demo Plugin

This plugin serves as a demonstration of basic plugin development in nopCommerce. It follows best practices and showcases common plugin patterns.

## Features

- Basic plugin structure and configuration
- Example of plugin settings
- Integration with nopCommerce services
- Basic admin interface
- Example of widget implementation

## Development

This plugin is developed following the architecture outlined in the [Plugin Architecture Documentation](../../PLUGIN_ARCHITECTURE.md).

### Project Structure

```
Nop.Plugin.Demo.BasicPlugin/
├── Controllers/           # MVC Controllers
├── Models/               # View Models
├── Views/                # Razor Views
├── Infrastructure/       # Plugin infrastructure
├── Services/            # Plugin-specific services
├── plugin.json          # Plugin metadata
└── DemoPlugin.cs        # Main plugin class
```

## Installation

1. Build the plugin
2. Copy the plugin to the nopCommerce plugins directory
3. Install the plugin through the admin interface

## Configuration

Configure the plugin through the admin interface at:
`Admin -> Configuration -> Local Plugins -> Demo Basic Plugin -> Configure`
