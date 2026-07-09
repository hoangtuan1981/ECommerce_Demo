import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { loginSchema, LoginForm } from '../schemas';
import { useLogin } from '../hooks/useLogin';
import { Button, Input, Card } from '@/components/ui';

export default function LoginPage() {
  const { mutate, isPending } = useLogin();
  const { register, handleSubmit, formState: { errors } } = useForm<LoginForm>({
    resolver: zodResolver(loginSchema),
  });

  const onSubmit = (data: LoginForm) => mutate(data);

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50">
      <Card className="w-full max-w-md p-8">
        <h1 className="text-3xl font-bold text-center mb-8">Đăng nhập</h1>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
          <div>
            <Input {...register('email')} placeholder="Email" type="email" />
            {errors.email && <p className="text-red-500 text-sm">{errors.email.message}</p>}
          </div>
          <div>
            <Input {...register('password')} placeholder="Mật khẩu" type="password" />
            {errors.password && <p className="text-red-500 text-sm">{errors.password.message}</p>}
          </div>
          <Button type="submit" className="w-full" disabled={isPending}>
            {isPending ? 'Đang đăng nhập...' : 'Đăng nhập'}
          </Button>
        </form>
      </Card>
    </div>
  );
}