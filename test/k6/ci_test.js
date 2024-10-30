import { check, sleep } from 'k6';
import http from 'k6/http';

export const options = {
    scenarios: {
        contacts: {
            executor: 'constant-vus',
            vus: 10,
            duration: '30s',
        },
    },
    thresholds: {
        http_req_failed: ['rate<0.01'],
        http_req_duration: ['p(95)<500'],
    },
};

export default function () {
    const response = http.get('https://quickpizza.grafana.com/');

    check(response, {
        'is status 200': (x) => x.status === 200
    });

    sleep(1);
}