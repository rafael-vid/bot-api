var Service = require('node-windows').Service;

// Create a new service object
var svc = new Service({
  name:'Bot_whats_passivo_001',
  script: './out/index.js',
});

svc.uninstall();