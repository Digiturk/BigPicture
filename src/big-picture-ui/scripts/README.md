## This script is created to initialize the project with correct versions.

Note: Firstly, please install [nvm-windows](https://github.com/coreybutler/nvm-windows/releases) and [gitbash](https://git-scm.com/downloads).

### Standart Usage

    ./init.sh

Note: If you'r windows user, please install [gitbash](https://git-scm.com/downloads) and run the bash app as admin/super user. (If you'r linux user, sudo su please :) ).

### Options

<table>
	<tr>
		<th>Param</th>
		<th>Description</th>
		<th>Default</th>
 	</tr>
 	<tr>
  		<td>-n</td>
   		<td>install target node version via NVM</td>
		<td>12.4.0</td>
 	</tr>
	<tr>
  		<td>-c</td>
   		<td> (boolean) clean npm cache</td>
		<td>false</td>
 	</tr>
</table>

#### Example Usage
Clean npm cache and Install and use node 14.5.0 :

        ./init.sh -n 14.5.0 -c
