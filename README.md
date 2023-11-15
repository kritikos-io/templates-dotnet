# Templates - Dotnet

A simple template leveraging [.Config][1] dotfiles submodule  for rapid project deployment. Simply rename Solution.{code-workspace,sln,sln.DotSettings} to your project name and get started! To use:

- Make sure git symlinks are enabled in your system
- Rename Solution.sln, Solution.sln.DotSettings and Solution.code-workspace to your project name (replacing only "Solution" on the file names)
- Edit [src/Directory.Build.props](src/Directory.Build.props) and add your project site URL (or your repository, if no site available)
- Enter a terminal inside the folder you just cloned and run `git submodule update --init`

You can replace the submodule with a compatible fork (to preserve your own default namespace etc) **provided it keeps file naming intact** since most files are appearing as symlinks.

[1]: https://github.com/kritikos-io/.config
