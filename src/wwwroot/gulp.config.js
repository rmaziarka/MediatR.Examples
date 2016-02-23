module.exports = function () {
    var root = './';
    var clientApp = root + 'app/';
    var specRunnerFile = 'specRunner.html';
    var bower = {
        json: require('./bower.json'),
        directory: './libs/',
        ignorePath: ''
    };

    var config = {
        bower: bower,
        jsOrder: [
            '**/app.module.js',
            '**/*.module.js',
            '**/*.js'
        ],
        jsTest: [
            clientApp + '**/*.module.js',
            clientApp + '**/*.module.config.js',
            clientApp + '**/*.module.const.js',
            clientApp + '**/*.js',
            '!' + clientApp + '**/*.spec.js',
            '!' + clientApp + '**/*.mock.js',
            '!' + clientApp + '**/*.route.js'
        ],
        root: root,
        specRunner: root + specRunnerFile,
        specsAndMocks: [clientApp + '**/*.spec.js', clientApp + '**/*.mock.js'],
        ts: {
            allTs: clientApp + '**/*.ts',
            output: clientApp,
            outputAllJs: clientApp + '**/*.js',
            outputAllJsMap: clientApp + '**/*.js.map',
            libTypingsAllTs: root + 'libs/typings/**/*.ts'
        },
        vendors: []
    }

    /**
     * wiredep and bower settings
     */
    config.getWiredepDefaultOptions = function () {
        var options = {
            bowerJson: config.bower.json,
            directory: config.bower.directory,
            ignorePath: config.bower.ignorePath
        };

        return options;
    };

    return config;
}