import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    stages: [
        { duration: '20s', target: 5 },   // ramp-up to 5 users
        { duration: '40s', target: 15 },  // stay at 10 users
        { duration: '30s', target: 0 },   // ramp-down
    ],
    thresholds: {
        http_req_duration: ['p(95)<500'],
        http_req_failed: ['rate<0.01'],
    },
};

export default function () {
    const url = 'https://localhost:7086/Employees/LoadEmployees'; // Change if you're using http or different port

    // Manually form-encode the body
    const payload =
        'draw=1' +
        '&start=1000' +
        '&length=10' +
        '&name=' +          // example filter
        '&phone=' +
        '&DepartmentId=2';

    const headers = {
        'Content-Type': 'application/x-www-form-urlencoded',
    };

    const res = http.post(url, payload, { headers });

    check(res, {
        'status is 200': (r) => r.status === 200,
        'has data': (r) => r.json('data') !== undefined,
    });

    sleep(1);
}
