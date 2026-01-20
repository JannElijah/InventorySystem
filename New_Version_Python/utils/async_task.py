import threading
from tkinter import messagebox

class AsyncTask:
    @staticmethod
    def run(func, on_success=None, on_error=None):
        """
        Runs 'func' in a separate thread.
        'func' should return a result.
        'on_success' (optional) is called on the main thread with the result.
        'on_error' (optional) is called on the main thread if an exception occurs.
        """
        def thread_target():
            try:
                result = func()
                if on_success:
                    # simplistic main thread dispatch (CTK is thread safe-ish for some updates, 
                    # but ideally we use after() or queued updates. 
                    # For simplicity in this scope, we call directly but handle with care)
                    on_success(result)
            except Exception as e:
                if on_error:
                    on_error(e)
                else:
                    print(f"Async Error: {e}")

        t = threading.Thread(target=thread_target, daemon=True)
        t.start()
