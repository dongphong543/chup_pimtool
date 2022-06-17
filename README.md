# chup_pimtool

Here are my 2 folders: Backend and Frontend. 

In the frontend, I've been facing with **a problem**.

- I have a service, BroadcaseService (https://github.com/dongphong543/chup_pimtool/blob/main/Frontend/src/app/Error/broadcast.service.ts), containing 2 BehaviorSubjects. The service is provided in 'root' (is it determined by the @Injectable decorator?), and I've not import/declare it anywhere else.

- In Error interceptor (https://github.com/dongphong543/chup_pimtool/blob/main/Frontend/src/app/Error/error-interceptor.ts), I feed those 2 BehaviorSubjects with the error and its status. The BroadcastService class has been injected into the interceptor class.

- In the error screen (https://github.com/dongphong543/chup_pimtool/blob/main/Frontend/src/app/Project/components/unexpected-error/unexpected-error.component.html), I take the BehaviorSubject's data out through the async pipe, but it doesn't work out. I've injected the BroadcastService class into the component's class.

The dataflow looks like this:

            Interceptor -------------> BroadcastService -------------> Error screen HTML

I hope with these information, you can help me with the problem. Thank you.

Ps. You can contact me through dongphong543@gmail.com.
