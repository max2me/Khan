# Khan
Inlines CSS making composing HTML emails much easier.

## How to use it?

1. Add stylesheets you need to inline and specify  `type="text/css-x"` on `<style>` tag
2. Use `new Unite().Process(string htmlSource)` to inline CSS

## Example

Input:
	
	<html>
		<head>
			<style type="text/css-x">
				table {
					width: 100%;
					+cellpadding: 0;
					+cellspacing: 0; 
				}
			</style>
		</head>
		<body>
			<table>
				<tr>
					<td>Hello, world!</td>
				</tr>
			</table>
		</body>
	</html>

Output:

	<html>
		<head></head>
		<body>
			<table cellpadding="0" cellspacing="0" style="width: 100%;">
				<tr>
					<td>Hello, world!</td>
				</tr>
			</table>
		</body>
	</html>

## Bonus feature
You can also add additional HTML attributes on tags by specifying rule "+" in front of the property name, it will then be added as an attribute instead of CSS property:

	table {
		width: 100%;
		+cellpadding: 0;
		+cellspacing: 0;
		+border: 0;
	}
