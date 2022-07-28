import { program } from 'commander';

import pkg from '../package.json';
// import Account from './account';

// const account = new Account();

program.version(pkg.version, '-v', '--version');

program
    .command('create [alias]')
    .description('Genereate new keypair')
    .action((alias) => {
        console.log('generate new account with alias', alias);
    });

program
    .command('import [alias]')
    .description('Import existing wallet')
    .option('-m <mnemonic>', '--mnemonic <mnemonic>', 'Mnemonic')
    .option('-p <privateKey>', '--privateKey <privateKey>', 'Private key')
    .action((alias, options) => {
        console.log('import account with alias', alias, ' and options', options);
    });

program
    .command('config [key] [value]')
    .description('Wallet configurations')
    .action((key, value) => {
        if (key && !value) {
            console.log('get config key');
        } else if (key && value) {
            console.log('set config key');
        }
    });

program
    .command('balance [alias]')
    .description('Get balance if account')
    .action((alias) => {
        console.log('get balance of account ', alias);
    });

program
    .command('send [alias] [amount] [recipient]')
    .description('Send transaction')
    .option('-b', '--broadcast', 'Broadcast tx to node')
    .option('-s', '--sign', 'Sign tx')
    .option('-o', '--output <file>', 'Output tx if not directly broadcasted')
    .option('-i', '--input <file>', 'Path to local tx file')
    .action((alias, amount, recipient, options) => {
        console.log('send', alias, amount, recipient, options);
    })


program.parse(process.argv);


if (!program.args.length) {
    program.help();
}