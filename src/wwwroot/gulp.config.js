module.exports = function () {
    var root = './';
    var clientApp = root + 'app/';
    var build = './build/';
    var specRunnerFile = 'specRunner.html';
    var translations = 'translations/';
    var bower = {
        json: require('./bower.json'),
        directory: './libs/',
        ignorePath: ''
    };
    var styles = root + 'styles/';

    var config = {
        index: 'index.html',
        specRunner: root + specRunnerFile,
        bower: bower,
        root: root,
        app: clientApp,
        js: {
            all: clientApp + '**/*.js',
            maps: clientApp + '**/*.js.map',
            appFiles: [
                clientApp + '**/*.js',
                '!' + clientApp + '**/*.spec.js',
                '!' + clientApp + '**/*.mock.js'
            ],
            appFilesToTest: [
                clientApp + '**/*.js',
                '!' + clientApp + '**/*.spec.js',
                '!' + clientApp + '**/*.mock.js',
                '!' + clientApp + '**/*.route.js',
                '!' + clientApp + '**/*.config.js',
            ],
            specsAndMocks: [
                clientApp + '**/*.spec.js',
                clientApp + '**/*.mock.js'
            ],
            order: [
                '**/app.module.js',
                '**/*.module.js',
                '**/*.js'
            ]
        },
        styles: {
            output: styles,
            sass: styles + '**/*.scss',
            css: styles + '*.css'
        },
        sassPaths: [
            bower.directory + 'bootstrap-sass/assets/stylesheets',
            bower.directory + 'font-awesome/scss',
        ],
        fonts: bower.directory + 'font-awesome/fonts/**/*.*',
        ts: {
            allTs: clientApp + '**/*.ts',
            libTypingsAllTs: root + 'typings/main/**/*.ts'
        },
        build: {
            output: build,
            temp: './.tmp/',
            testInput: './.test/',
            templateCache: {
                file: 'templates.js',
                options: {
                    module: 'app',
                    standAlone: false,
                    root: 'app/'
                }
            },
            htmlTemplatesSrc: clientApp + '**/*.html',
            translationsDest: build + translations,
            staticTranslationsFiles: [
                translations + '*.json', // translation              
            ],
            staticAppFiles: [
                root + 'wwwroot.nuspec',
                root + 'app.json',  // tokens
            ],
            webConfigFile: [
                root + '_web.config'
            ],
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
